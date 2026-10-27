using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using Microsoft.AspNetCore.Http;

namespace EmirCoaching.Business.Services.Abstract;

public interface IUploadService
{
    // Admin: görsel yükler (jpg, jpeg, png, webp; en fazla 5 MB). Geçersiz dosya → Invalid.
    // folder: uploads altındaki klasör (ör. "blog"); verilmezse "images"
    Task<Result<UploadedFileResponse>> UploadImageAsync(IFormFile file, string? folder);
}
