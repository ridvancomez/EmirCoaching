using AwesomeAssertions;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.Tests.Repositories;

public class UploadReferenceRepositoryTests : IDisposable
{
    private readonly SqliteDatabase _db = new();

    public void Dispose() => _db.Dispose();

    // UploadReferenceRepository'nin baktığı kolonlar (Tablo.Kolon). Yeni bir *Path kolonu eklenince bu test kırılır:
    // repository'ye eklenmezse o kolonun dosyaları günlük temizlikte silinir. Ekledikten sonra buraya da yazılır.
    private static readonly string[] CoveredPathColumns =
    [
        "BlogPost.CoverImagePath",
        "Transformation.BeforeImagePath",
        "Transformation.AfterImagePath",
        "Transformation.BeforeSquarePath",
        "Transformation.AfterSquarePath",
        "SocialPost.ImagePath",
        "Certificate.LogoPath",
        "PageContent.BackgroundImagePath",
        "DietProgram.DietitianPhotoPath",
        "AppUser.AvatarPath",
        "LegalDocument.FilePath"
    ];

    [Fact]
    public void CoveredPathColumns_MatchEveryPathColumnInModel()
    {
        // Arrange
        using var context = _db.CreateContext();

        // Act
        var modelColumns = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties()
                .Where(p => p.ClrType == typeof(string) && p.Name.EndsWith("Path", StringComparison.Ordinal))
                .Select(p => $"{t.ClrType.Name}.{p.Name}"))
            .ToList();

        // Assert
        modelColumns.Should().BeEquivalentTo(CoveredPathColumns);
    }

    [Fact]
    public async Task GetUsedPathsAsync_IncludesPathsOfActiveRecords()
    {
        // Arrange
        await using (var context = _db.CreateContext())
        {
            context.SocialPosts.Add(new SocialPost { Url = "https://youtube.com/a", ImagePath = "/uploads/video/a.jpg" });
            await context.SaveChangesAsync();
        }

        // Act
        await using var check = _db.CreateContext();
        var used = await new UploadReferenceRepository(check).GetUsedPathsAsync();

        // Assert — karşılaştırma büyük/küçük harf duyarsız
        used.Should().Contain("/uploads/video/a.jpg");
        used.Contains("/UPLOADS/video/A.jpg").Should().BeTrue();
    }

    [Fact]
    public async Task GetUsedPathsAsync_ExcludesSoftDeletedRecordsAndSiteImages()
    {
        // Arrange
        await using (var context = _db.CreateContext())
        {
            var deleted = new SocialPost { Url = "https://youtube.com/s", ImagePath = "/uploads/video/silinen.jpg" };
            context.SocialPosts.AddRange(deleted, new SocialPost { Url = "https://youtube.com/i", ImagePath = "/images/site.jpg" });
            await context.SaveChangesAsync();
            context.SocialPosts.Remove(deleted);
            await context.SaveChangesAsync();
        }

        // Act
        await using var check = _db.CreateContext();
        var used = await new UploadReferenceRepository(check).GetUsedPathsAsync();

        // Assert
        used.Should().NotContain("/uploads/video/silinen.jpg");
        used.Should().NotContain("/images/site.jpg");
    }

    [Fact]
    public async Task GetUsedPathsAsync_IncludesLegalDocumentsEvenWhenDeleted()
    {
        // Arrange — yasal metin PDF'leri (eski sürümler dahil) hiç silinmez
        await using (var context = _db.CreateContext())
        {
            var document = new LegalDocument
            {
                Type = LegalDocumentType.Cerez, Title = "Eski", Version = 99, Note = "Not",
                FilePath = "/uploads/yasal-metinler/eski.pdf", PublishedAt = DateTime.UtcNow
            };
            context.LegalDocuments.Add(document);
            await context.SaveChangesAsync();
            context.LegalDocuments.Remove(document);
            await context.SaveChangesAsync();
        }

        // Act
        await using var check = _db.CreateContext();
        var used = await new UploadReferenceRepository(check).GetUsedPathsAsync();

        // Assert
        used.Should().Contain("/uploads/yasal-metinler/eski.pdf");
    }
}
