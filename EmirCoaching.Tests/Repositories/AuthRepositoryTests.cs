using AwesomeAssertions;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;
using EmirCoaching.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.Tests.Repositories;

// Oturum ve giriş geçmişi kayıtları: günlük bakımdaki kalıcı silmeler ve aktif oturum sorgusu
public class AuthRepositoryTests : IDisposable
{
    private readonly SqliteDatabase _db = new();
    private readonly int _userId;

    public AuthRepositoryTests()
    {
        using var context = _db.CreateContext();
        var user = new AppUser { UserName = "admin@test", Email = "admin@test", FullName = "Admin" };
        context.Users.Add(user);
        context.SaveChanges();
        _userId = user.Id;
    }

    public void Dispose() => _db.Dispose();

    private async Task AddTokensAsync(params RefreshToken[] tokens)
    {
        await using var context = _db.CreateContext();
        foreach (var token in tokens)
            token.UserId = _userId;
        context.RefreshTokens.AddRange(tokens);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetActiveByUserIdAsync_ExcludesRevokedAndExpiredTokens()
    {
        // Arrange
        await AddTokensAsync(
            new RefreshToken { TokenHash = "aktif", ExpiresAt = DateTime.UtcNow.AddDays(1) },
            new RefreshToken { TokenHash = "iptal", ExpiresAt = DateTime.UtcNow.AddDays(1), RevokedAt = DateTime.UtcNow },
            new RefreshToken { TokenHash = "suresi-dolmus", ExpiresAt = DateTime.UtcNow.AddDays(-1) });

        // Act
        await using var context = _db.CreateContext();
        var active = await new RefreshTokenRepository(context).GetActiveByUserIdAsync(_userId);

        // Assert
        active.Select(t => t.TokenHash).Should().Equal("aktif");
    }

    [Fact]
    public async Task GetByHashAsync_IncludesUser()
    {
        // Arrange — RefreshAsync yeni token'ı bu kullanıcıyla üretir
        await AddTokensAsync(new RefreshToken { TokenHash = "abc", ExpiresAt = DateTime.UtcNow.AddDays(1) });

        // Act
        await using var context = _db.CreateContext();
        var token = await new RefreshTokenRepository(context).GetByHashAsync("abc");

        // Assert
        token!.User.Should().NotBeNull();
        token.User.Id.Should().Be(_userId);
    }

    [Fact]
    public async Task DeletePermanentlyExpiredBeforeAsync_DeletesOnlyTokensExpiredBeforeDate()
    {
        // Arrange — iptal edilmiş ama süresi dolmamış token tutulur (tekrar kullanım yakalansın)
        await AddTokensAsync(
            new RefreshToken { TokenHash = "eski", ExpiresAt = DateTime.UtcNow.AddDays(-40) },
            new RefreshToken { TokenHash = "iptal-yeni", ExpiresAt = DateTime.UtcNow.AddDays(3), RevokedAt = DateTime.UtcNow });

        // Act
        int deleted;
        await using (var context = _db.CreateContext())
            deleted = await new RefreshTokenRepository(context).DeletePermanentlyExpiredBeforeAsync(DateTime.UtcNow.AddDays(-30));

        // Assert
        await using var check = _db.CreateContext();
        deleted.Should().Be(1);
        (await check.RefreshTokens.IgnoreQueryFilters().Select(t => t.TokenHash).ToListAsync()).Should().Equal("iptal-yeni");
    }

    [Fact]
    public async Task DeletePermanentlyBeforeAsync_DeletesOldLoginAttempts()
    {
        // Arrange
        await using (var context = _db.CreateContext())
        {
            context.LoginAttempts.AddRange(
                new LoginAttempt { Result = LoginAttemptResult.Basarili, IpAddress = "1", CreatedAt = DateTime.UtcNow.AddDays(-100) },
                new LoginAttempt { Result = LoginAttemptResult.HataliSifre, IpAddress = "2" });
            await context.SaveChangesAsync();
        }

        // Act
        int deleted;
        await using (var context = _db.CreateContext())
            deleted = await new LoginAttemptRepository(context).DeletePermanentlyBeforeAsync(DateTime.UtcNow.AddDays(-90));

        // Assert
        await using var check = _db.CreateContext();
        deleted.Should().Be(1);
        (await check.LoginAttempts.IgnoreQueryFilters().SingleAsync()).IpAddress.Should().Be("2");
    }
}
