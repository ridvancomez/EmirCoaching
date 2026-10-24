using Microsoft.AspNetCore.Http;

namespace EmirCoaching.Business.Services.Abstract;

// Dosyalar wwwroot/uploads altına kaydedilir; dönen değer site köküne göre yoldur (/uploads/...).
// Geçersiz dosyada ArgumentException fırlatır.
public interface IFileStorageService
{
    Task<string> SaveImageAsync(IFormFile file, string? folder);
    Task<string> SavePdfAsync(IFormFile file, string? folder);
    Task DeleteAsync(string filePath);
}
