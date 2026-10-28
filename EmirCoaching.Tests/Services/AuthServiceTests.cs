using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;
using EmirCoaching.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace EmirCoaching.Tests.Services;

public class AuthServiceTests
{
    private const string Password = "Gizli-Sifre-123";
    private static readonly RequestInfo Info = new("10.0.0.1", "Test tarayıcı");

    private readonly Mock<UserManager<AppUser>> _userManager = TestUserManager.Create();
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokens = new();
    private readonly Mock<ILoginAttemptRepository> _loginAttempts = new();
    private readonly Mock<IJwtTokenService> _tokens = new();
    private readonly AppUser _user = new() { Id = 1, Email = "admin@emircoaching.com", FullName = "Emir" };
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _uow.Setup(u => u.RefreshTokens).Returns(_refreshTokens.Object);
        _uow.Setup(u => u.LoginAttempts).Returns(_loginAttempts.Object);

        // Token üretimi ayrı serviste (JwtTokenService); burada sabit değerler döner
        _tokens.Setup(t => t.CreateAccessToken(It.IsAny<AppUser>(), It.IsAny<IEnumerable<string>>()))
            .Returns(new AccessToken("access-token", DateTime.UtcNow.AddMinutes(15)));
        _tokens.Setup(t => t.CreateRefreshToken()).Returns("yeni-refresh");
        _tokens.Setup(t => t.HashRefreshToken(It.IsAny<string>())).Returns((string t) => "hash:" + t);

        _userManager.Setup(m => m.FindByEmailAsync(_user.Email!)).ReturnsAsync(_user);
        _userManager.Setup(m => m.CheckPasswordAsync(_user, Password)).ReturnsAsync(true);
        _userManager.Setup(m => m.GetRolesAsync(_user)).ReturnsAsync(["Admin"]);

        _service = new AuthService(_userManager.Object, _uow.Object, _tokens.Object, TestMapper.Create(),
            Options.Create(new JwtOptions { RefreshTokenDays = 7 }), new LoginRequestValidator(),
            NullLogger<AuthService>.Instance);
    }

    private static LoginRequest Login(string email = "admin@emircoaching.com", string password = Password) =>
        new() { Email = email, Password = password };

    // ---- LoginAsync ----

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsTokensAndStoresRefreshTokenHash()
    {
        // Act
        var result = await _service.LoginAsync(Login(), Info);

        // Assert — DB'ye token'ın kendisi değil özeti yazılır
        result.IsSuccess.Should().BeTrue();
        result.Value!.AccessToken.Token.Should().Be("access-token");
        result.Value.RefreshToken.Should().Be("yeni-refresh");
        _refreshTokens.Verify(r => r.AddAsync(It.Is<RefreshToken>(t =>
            t.TokenHash == "hash:yeni-refresh" && t.UserId == 1 && t.CreatedByIp == Info.IpAddress)), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_SetsRefreshExpiryFromOptions()
    {
        // Act
        var result = await _service.LoginAsync(Login(), Info);

        // Assert
        result.Value!.RefreshTokenExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddDays(7), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ResetsFailedCountAndRecordsSuccess()
    {
        // Act
        await _service.LoginAsync(Login(), Info);

        // Assert
        _userManager.Verify(m => m.ResetAccessFailedCountAsync(_user), Times.Once);
        _loginAttempts.Verify(r => r.AddAsync(It.Is<LoginAttempt>(a => a.Result == LoginAttemptResult.Basarili && a.UserId == 1)), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithUnknownEmail_ReturnsSameMessageAsWrongPassword()
    {
        // Act
        var unknown = await _service.LoginAsync(Login(email: "yok@example.com"), Info);
        var wrongPassword = await _service.LoginAsync(Login(password: "yanlis-sifre"), Info);

        // Assert — kullanıcının var olup olmadığı yanıttan anlaşılmamalı
        unknown.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        unknown.Errors.Should().Equal(wrongPassword.Errors);
    }

    [Fact]
    public async Task LoginAsync_WithUnknownEmail_RecordsAttemptWithoutUser()
    {
        // Act
        await _service.LoginAsync(Login(email: "yok@example.com"), Info);

        // Assert — bulunamayan e-posta giriş geçmişine yazılmaz (LoginAttempt'te e-posta alanı yok, kullanıcı boş)
        _loginAttempts.Verify(r => r.AddAsync(It.Is<LoginAttempt>(a => a.Result == LoginAttemptResult.KullaniciYok && a.UserId == null)), Times.Once);
        _refreshTokens.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_CountsFailedAttempt()
    {
        // Act
        var result = await _service.LoginAsync(Login(password: "yanlis-sifre"), Info);

        // Assert — Identity 5 hatalı denemede hesabı kilitler
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        _userManager.Verify(m => m.AccessFailedAsync(_user), Times.Once);
        _loginAttempts.Verify(r => r.AddAsync(It.Is<LoginAttempt>(a => a.Result == LoginAttemptResult.HataliSifre && a.UserId == 1)), Times.Once);
        _refreshTokens.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WhenLockedOut_RejectsWithoutCheckingPassword()
    {
        // Arrange
        _userManager.Setup(m => m.IsLockedOutAsync(_user)).ReturnsAsync(true);

        // Act — şifre doğru olsa bile
        var result = await _service.LoginAsync(Login(), Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        _userManager.Verify(m => m.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Never);
        _loginAttempts.Verify(r => r.AddAsync(It.Is<LoginAttempt>(a => a.Result == LoginAttemptResult.Kilitli)), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithLongUserAgent_TruncatesTo512Characters()
    {
        // Act
        await _service.LoginAsync(Login(), Info with { UserAgent = new string('a', 1000) });

        // Assert
        _loginAttempts.Verify(r => r.AddAsync(It.Is<LoginAttempt>(a => a.UserAgent.Length == 512)), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyFields_ReturnsValidationErrorAndDoesNotRecordAttempt()
    {
        // Act
        var result = await _service.LoginAsync(Login(email: "", password: ""), Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _loginAttempts.Verify(r => r.AddAsync(It.IsAny<LoginAttempt>()), Times.Never);
    }

    // ---- RefreshAsync ----

    [Fact]
    public async Task RefreshAsync_WithActiveToken_RevokesItAndIssuesNewOne()
    {
        // Arrange
        var stored = StoredToken();
        _refreshTokens.Setup(r => r.GetByHashAsync("hash:eski-refresh")).ReturnsAsync(stored);

        // Act
        var result = await _service.RefreshAsync("eski-refresh", Info);

        // Assert — rotation: eskisi iptal, yenisi kaydedildi
        result.IsSuccess.Should().BeTrue();
        result.Value!.RefreshToken.Should().Be("yeni-refresh");
        stored.RevokedAt.Should().NotBeNull();
        _refreshTokens.Verify(r => r.AddAsync(It.Is<RefreshToken>(t => t.TokenHash == "hash:yeni-refresh")), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_WithRevokedToken_RevokesAllUserSessions()
    {
        // Arrange — iptal edilmiş token tekrar geldi (çalınmış olabilir)
        var reused = StoredToken(revokedAt: DateTime.UtcNow.AddMinutes(-5));
        var otherSession = StoredToken();
        _refreshTokens.Setup(r => r.GetByHashAsync("hash:eski-refresh")).ReturnsAsync(reused);
        _refreshTokens.Setup(r => r.GetActiveByUserIdAsync(1)).ReturnsAsync([otherSession]);

        // Act
        var result = await _service.RefreshAsync("eski-refresh", Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        otherSession.RevokedAt.Should().NotBeNull();
        _refreshTokens.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task RefreshAsync_WithExpiredToken_ReturnsUnauthorized()
    {
        // Arrange
        _refreshTokens.Setup(r => r.GetByHashAsync("hash:eski-refresh"))
            .ReturnsAsync(StoredToken(expiresAt: DateTime.UtcNow.AddMinutes(-1)));

        // Act
        var result = await _service.RefreshAsync("eski-refresh", Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        _refreshTokens.Verify(r => r.AddAsync(It.IsAny<RefreshToken>()), Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("bilinmeyen-token")]
    public async Task RefreshAsync_WithMissingOrUnknownToken_ReturnsUnauthorized(string? token)
    {
        // Act
        var result = await _service.RefreshAsync(token, Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Unauthorized);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    // ---- LogoutAsync ----

    [Fact]
    public async Task LogoutAsync_WithActiveToken_RevokesIt()
    {
        // Arrange
        var stored = StoredToken();
        _refreshTokens.Setup(r => r.GetByHashAsync("hash:eski-refresh")).ReturnsAsync(stored);

        // Act
        var result = await _service.LogoutAsync("eski-refresh");

        // Assert
        result.IsSuccess.Should().BeTrue();
        stored.RevokedAt.Should().NotBeNull();
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task LogoutAsync_WithAlreadyRevokedToken_KeepsOriginalRevokeDate()
    {
        // Arrange
        var revokedAt = DateTime.UtcNow.AddHours(-1);
        var stored = StoredToken(revokedAt: revokedAt);
        _refreshTokens.Setup(r => r.GetByHashAsync("hash:eski-refresh")).ReturnsAsync(stored);

        // Act
        var result = await _service.LogoutAsync("eski-refresh");

        // Assert
        result.IsSuccess.Should().BeTrue();
        stored.RevokedAt.Should().Be(revokedAt);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task LogoutAsync_WithoutToken_Succeeds()
    {
        // Act
        var result = await _service.LogoutAsync(null);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    // ---- GetCurrentUserAsync ----

    [Fact]
    public async Task GetCurrentUserAsync_WithExistingUser_ReturnsUserWithRoles()
    {
        // Arrange
        _userManager.Setup(m => m.FindByIdAsync("1")).ReturnsAsync(_user);

        // Act
        var result = await _service.GetCurrentUserAsync(1);

        // Assert
        result.Value.Should().BeEquivalentTo(new CurrentUserResponse
        {
            Id = 1, Email = "admin@emircoaching.com", FullName = "Emir", Roles = ["Admin"]
        });
    }

    [Fact]
    public async Task GetCurrentUserAsync_WithMissingUser_ReturnsNotFound()
    {
        // Act
        var result = await _service.GetCurrentUserAsync(99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    private RefreshToken StoredToken(DateTime? expiresAt = null, DateTime? revokedAt = null) => new()
    {
        TokenHash = "hash:eski-refresh",
        ExpiresAt = expiresAt ?? DateTime.UtcNow.AddDays(3),
        RevokedAt = revokedAt,
        UserId = _user.Id,
        User = _user
    };
}
