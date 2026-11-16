using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Görsel önce buraya yüklenir, dönen path kayıt isteğindeki *Path alanına yazılır.
// PDF yalnızca yasal metin ucundan yüklenir.
[Route("api/v1/admin/uploads")]
public class UploadsController(IUploadService uploadService) : AdminControllerBase
{
    private const long MaxRequestBytes = 6 * 1024 * 1024; // görsel en fazla 5 MB + form alanları

    // multipart/form-data: file + isteğe bağlı folder (ör. "blog") → 200 { path, url } / 400
    [HttpPost("images")]
    [RequestSizeLimit(MaxRequestBytes)]
    public async Task<ActionResult<UploadedFileResponse>> UploadImage(IFormFile file, [FromForm] string? folder) =>
        FromResult(await uploadService.UploadImageAsync(file, folder));
}
