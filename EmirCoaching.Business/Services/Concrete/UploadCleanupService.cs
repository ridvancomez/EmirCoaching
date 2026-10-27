using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class UploadCleanupService(
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorage,
    ILogger<UploadCleanupService> logger) : IUploadCleanupService
{
    public async Task DeleteUnusedAsync(IEnumerable<string> paths)
    {
        var candidates = paths.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        if (candidates.Count == 0)
            return;

        var deleted = await DeleteIfUnusedAsync(candidates);
        if (deleted > 0)
            logger.LogInformation("Kullanılmayan {Count} görsel silindi", deleted);
    }

    public async Task<int> DeleteOrphansAsync(DateTime modifiedBefore) =>
        await DeleteIfUnusedAsync(await fileStorage.ListAsync(modifiedBefore));

    private async Task<int> DeleteIfUnusedAsync(IReadOnlyCollection<string> paths)
    {
        if (paths.Count == 0)
            return 0;

        var used = await unitOfWork.UploadReferences.GetUsedPathsAsync();
        var unused = paths.Where(p => !used.Contains(p)).ToList();

        foreach (var path in unused)
            await fileStorage.DeleteAsync(path);

        return unused.Count;
    }
}
