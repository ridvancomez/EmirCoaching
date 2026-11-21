using AwesomeAssertions;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.Entities.Identity;
using EmirCoaching.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.Tests.Repositories;

// Sitedeki koç bölümü IsCoach işaretli tek kullanıcıdan gelir
public class AppUserRepositoryTests : IDisposable
{
    private readonly SqliteDatabase _db = new();

    public void Dispose() => _db.Dispose();

    private static AppUser User(string email, bool isCoach) =>
        new() { UserName = email, Email = email, FullName = email, IsCoach = isCoach };

    [Fact]
    public async Task GetCoachAsync_ReturnsUserMarkedAsCoach()
    {
        // Arrange
        await using (var context = _db.CreateContext())
        {
            context.Users.AddRange(User("editor@test", isCoach: false), User("koc@test", isCoach: true));
            await context.SaveChangesAsync();
        }

        // Act
        await using var query = _db.CreateContext();
        var coach = await new AppUserRepository(query).GetCoachAsync();

        // Assert
        coach!.Email.Should().Be("koc@test");
    }

    [Fact]
    public async Task SaveChanges_WithSecondCoach_ViolatesUniqueIndex()
    {
        // Arrange
        await using var context = _db.CreateContext();
        context.Users.AddRange(User("koc1@test", isCoach: true), User("koc2@test", isCoach: true));

        // Act
        var act = () => context.SaveChangesAsync();

        // Assert
        await act.Should().ThrowAsync<DbUpdateException>();
    }
}
