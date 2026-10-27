using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class UploadReferenceRepository(AppDbContext context) : IUploadReferenceRepository
{
    private const string UploadsPrefix = "/uploads/";

    public async Task<HashSet<string>> GetUsedPathsAsync()
    {
        // Tek SQL (UNION ALL); soft delete'li kayıtların dosyaları kullanılmıyor sayılır
        var paths = await context.BlogPosts.Select(x => x.CoverImagePath)
            .Concat(context.Transformations.Select(x => (string?)x.BeforeImagePath))
            .Concat(context.Transformations.Select(x => (string?)x.AfterImagePath))
            .Concat(context.Transformations.Select(x => x.BeforeSquarePath))
            .Concat(context.Transformations.Select(x => x.AfterSquarePath))
            .Concat(context.VideoQuestions.Select(x => (string?)x.ThumbnailPath))
            .Concat(context.SocialPosts.Select(x => (string?)x.ImagePath))
            .Concat(context.PageContents.Select(x => x.BackgroundImagePath))
            .Concat(context.DietPrograms.Select(x => x.DietitianPhotoPath))
            .Concat(context.Users.Select(x => x.AvatarPath))
            .Where(p => p != null && p.StartsWith(UploadsPrefix))
            .Distinct()
            .ToListAsync();

        // Ayrı sorgu: IgnoreQueryFilters Concat içinde kullanılırsa tüm sorgunun filtresini kapatır
        var legalPaths = await context.LegalDocuments.IgnoreQueryFilters()
            .Where(x => x.FilePath != null && x.FilePath.StartsWith(UploadsPrefix))
            .Select(x => x.FilePath)
            .ToListAsync();

        return paths.Concat(legalPaths).Select(p => p!).ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
}
