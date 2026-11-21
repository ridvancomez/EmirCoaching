using AwesomeAssertions;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.Tests.Repositories;

// Paket soft delete olunca başvuru listede kalır ve paketi hâlâ gelir (panel soluk gösterir)
public class DietFormSubmissionRepositoryTests : IDisposable
{
    private const int SeededPackageId = 1;

    private readonly SqliteDatabase _db = new();

    public void Dispose() => _db.Dispose();

    private async Task AddSubmissionAsync(string email)
    {
        await using var context = _db.CreateContext();
        context.DietFormSubmissions.Add(new DietFormSubmission
        {
            PackageId = SeededPackageId, FullName = "Ayşe Yılmaz", Email = email, PhoneNumber = "05551112233"
        });
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetPagedAsync_WithSoftDeletedPackage_ReturnsSubmissionWithPackage()
    {
        // Arrange
        await AddSubmissionAsync("ayse@example.com");
        await using (var context = _db.CreateContext())
        {
            context.Packages.Remove(await context.Packages.SingleAsync(p => p.Id == SeededPackageId));
            await context.SaveChangesAsync();
        }

        // Act
        await using var query = _db.CreateContext();
        var page = await new DietFormSubmissionRepository(query).GetPagedAsync(null, 1, 20);

        // Assert
        var submission = page.Items.Should().ContainSingle().Subject;
        submission.Package.Should().NotBeNull();
        submission.Package.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task GetPagedAsync_WithSoftDeletedSubmission_ExcludesIt()
    {
        // Arrange — kapatılan query filter başvurunun kendi soft delete'ini de açmasın
        await AddSubmissionAsync("ayse@example.com");
        await AddSubmissionAsync("ali@example.com");
        await using (var context = _db.CreateContext())
        {
            context.DietFormSubmissions.Remove(await context.DietFormSubmissions.SingleAsync(s => s.Email == "ali@example.com"));
            await context.SaveChangesAsync();
        }

        // Act
        await using var query = _db.CreateContext();
        var page = await new DietFormSubmissionRepository(query).GetPagedAsync(DietFormStatus.Yeni, 1, 20);

        // Assert
        page.Items.Select(s => s.Email).Should().Equal("ayse@example.com");
    }
}
