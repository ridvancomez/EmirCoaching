using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmirCoaching.Business.Services.Concrete;

public class UploadService(
    IFileStorageService fileStorage,
    IOptions<StorageOptions> storageOptions,
    ILogger<UploadService> logger) : IUploadService
{
    public async Task<Result<UploadedFileResponse>> UploadImageAsync(IFormFile file, string? folder)
    {
        string path;
        try
        {
            path = await fileStorage.SaveImageAsync(file, string.IsNullOrWhiteSpace(folder) ? null : folder.Trim());
        }
        catch (ArgumentException ex) // dosya kurallarına uymayan yükleme (boyut, tür, içerik imzası)
        {
            logger.LogWarning("Görsel yükleme reddedildi: {Reason}", ex.Message);
            return Result<UploadedFileResponse>.Invalid(ex.Message);
        }

        logger.LogInformation("Görsel yüklendi: {Path}", path);
        return Result<UploadedFileResponse>.Success(new UploadedFileResponse
        {
            Path = path,
            Url = storageOptions.Value.ToPublicUrl(path)
        });
    }
}
