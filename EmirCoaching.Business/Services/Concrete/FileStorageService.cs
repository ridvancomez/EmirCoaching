using System.Text.RegularExpressions;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EmirCoaching.Business.Services.Concrete;

public partial class FileStorageService(IWebHostEnvironment environment) : IFileStorageService
{
    private const string UploadsFolder = "uploads";
    private const long MaxImageBytes = 5 * 1024 * 1024;
    private const long MaxPdfBytes = 10 * 1024 * 1024;

    // Uzantı → dosya imzası (magic bytes); uzantı tek başına güvenilmez
    private static readonly Dictionary<string, byte[]> ImageSignatures = new()
    {
        [".jpg"] = [0xFF, 0xD8, 0xFF],
        [".jpeg"] = [0xFF, 0xD8, 0xFF],
        [".png"] = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A],
        [".webp"] = "RIFF"u8.ToArray() // + 8. bayttan itibaren "WEBP", aşağıda kontrol edilir
    };

    private static readonly byte[] PdfSignature = "%PDF-"u8.ToArray();

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex FolderPattern();

    private string WebRoot => environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");

    public Task<string> SaveImageAsync(IFormFile file, string? folder) =>
        SaveAsync(file, folder ?? "images", MaxImageBytes, extension =>
            ImageSignatures.TryGetValue(extension, out var signature) ? signature : null);

    public Task<string> SavePdfAsync(IFormFile file, string? folder) =>
        SaveAsync(file, folder ?? "documents", MaxPdfBytes, extension =>
            extension == ".pdf" ? PdfSignature : null);

    public Task DeleteAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Task.CompletedTask;

        var uploadsRoot = Path.GetFullPath(Path.Combine(WebRoot, UploadsFolder)) + Path.DirectorySeparatorChar;
        var fullPath = Path.GetFullPath(Path.Combine(WebRoot, filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar)));

        // Yalnızca uploads altındaki dosyalar silinebilir (path traversal koruması)
        if (!fullPath.StartsWith(uploadsRoot, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Yalnızca yüklenen dosyalar silinebilir.", nameof(filePath));

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

    private async Task<string> SaveAsync(IFormFile file, string folder, long maxBytes, Func<string, byte[]?> signatureFor)
    {
        if (file is null || file.Length == 0)
            throw new ArgumentException("Dosya boş.", nameof(file));
        if (file.Length > maxBytes)
            throw new ArgumentException($"Dosya en fazla {maxBytes / 1024 / 1024} MB olabilir.", nameof(file));
        if (!FolderPattern().IsMatch(folder))
            throw new ArgumentException("Geçersiz klasör adı.", nameof(folder));

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var signature = signatureFor(extension)
            ?? throw new ArgumentException($"Desteklenmeyen dosya türü: {extension}", nameof(file));

        var header = new byte[12];
        int read;
        await using (var headerStream = file.OpenReadStream())
            read = await headerStream.ReadAtLeastAsync(header, header.Length, throwOnEndOfStream: false);

        var validSignature = read >= signature.Length && header.AsSpan(0, signature.Length).SequenceEqual(signature)
            && (extension != ".webp" || (read >= 12 && header.AsSpan(8, 4).SequenceEqual("WEBP"u8)));
        if (!validSignature)
            throw new ArgumentException("Dosya içeriği uzantısıyla uyuşmuyor.", nameof(file));

        var directory = Path.Combine(WebRoot, UploadsFolder, folder);
        Directory.CreateDirectory(directory);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        await using (var output = new FileStream(Path.Combine(directory, fileName), FileMode.CreateNew))
            await file.CopyToAsync(output);

        return $"/{UploadsFolder}/{folder}/{fileName}";
    }
}
