using AwesomeAssertions;
using EmirCoaching.Business.Services.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EmirCoaching.Tests.Services;

// Gerçek dosya sistemiyle çalışır: her test kendi geçici wwwroot klasörünü kullanır ve sonunda siler
public class FileStorageServiceTests : IDisposable
{
    private static readonly byte[] Jpeg = [0xFF, 0xD8, 0xFF, 0xE0, 0, 0, 0, 0, 0, 0, 0, 0];
    private static readonly byte[] Png = [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0, 0, 0, 0];
    private static readonly byte[] Webp = [.. "RIFF"u8, 0, 0, 0, 0, .. "WEBP"u8];
    private static readonly byte[] Pdf = [.. "%PDF-1.7"u8, 0, 0, 0, 0];

    private readonly string _webRoot = Path.Combine(Path.GetTempPath(), "emircoaching-tests", Guid.NewGuid().ToString("N"));
    private readonly FileStorageService _service;

    public FileStorageServiceTests()
    {
        Directory.CreateDirectory(_webRoot);
        _service = new FileStorageService(Mock.Of<IWebHostEnvironment>(e => e.WebRootPath == _webRoot));
    }

    // xUnit her testten sonra Dispose'u çağırır
    public void Dispose() => Directory.Delete(_webRoot, recursive: true);

    private static FormFile File(string fileName, byte[] content) =>
        new(new MemoryStream(content), 0, content.Length, "file", fileName);

    private string FullPath(string storedPath) => Path.Combine(_webRoot, storedPath.TrimStart('/'));

    // ---- SaveImageAsync ----

    [Theory]
    [InlineData("foto.jpg")]
    [InlineData("foto.JPEG")]
    [InlineData("foto.png")]
    [InlineData("foto.webp")]
    public async Task SaveImageAsync_WithValidImage_SavesUnderUploadsWithRandomName(string fileName)
    {
        // Arrange
        var content = Path.GetExtension(fileName).ToLowerInvariant() switch { ".png" => Png, ".webp" => Webp, _ => Jpeg };

        // Act
        var path = await _service.SaveImageAsync(File(fileName, content), "blog");

        // Assert — kullanıcının dosya adı kullanılmaz
        path.Should().MatchRegex(@"^/uploads/blog/[0-9a-f]{32}\.(jpg|jpeg|png|webp)$");
        System.IO.File.Exists(FullPath(path)).Should().BeTrue();
    }

    [Fact]
    public async Task SaveImageAsync_WithoutFolder_UsesImagesFolder()
    {
        // Act
        var path = await _service.SaveImageAsync(File("a.jpg", Jpeg), null);

        // Assert
        path.Should().StartWith("/uploads/images/");
    }

    [Theory]
    [InlineData("zararli.jpg", "Png")]      // uzantı jpg, içerik png
    [InlineData("zararli.png", "Pdf")]
    [InlineData("zararli.webp", "Riff")]    // RIFF ama WEBP değil (ör. .wav)
    public async Task SaveImageAsync_WithContentNotMatchingExtension_Throws(string fileName, string contentKind)
    {
        // Arrange
        byte[] content = contentKind switch
        {
            "Png" => Png,
            "Pdf" => Pdf,
            _ => [.. "RIFF"u8, 0, 0, 0, 0, .. "WAVE"u8]
        };

        // Act
        var act = () => _service.SaveImageAsync(File(fileName, content), "blog");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Dosya içeriği uzantısıyla uyuşmuyor.");
    }

    [Theory]
    [InlineData("script.svg")]   // SVG içinde script olabilir
    [InlineData("foto.gif")]
    [InlineData("foto")]
    public async Task SaveImageAsync_WithUnsupportedExtension_Throws(string fileName)
    {
        // Act
        var act = () => _service.SaveImageAsync(File(fileName, Jpeg), "blog");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Desteklenmeyen dosya türü*");
    }

    [Fact]
    public async Task SaveImageAsync_LargerThan5Mb_Throws()
    {
        // Arrange
        var content = new byte[5 * 1024 * 1024 + 1];
        Jpeg.CopyTo(content, 0);

        // Act
        var act = () => _service.SaveImageAsync(File("buyuk.jpg", content), "blog");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Dosya en fazla 5 MB olabilir.");
    }

    [Fact]
    public async Task SaveImageAsync_WithEmptyFile_Throws()
    {
        // Act
        var act = () => _service.SaveImageAsync(File("bos.jpg", []), "blog");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Dosya boş.");
    }

    [Theory]
    [InlineData("../disari")]
    [InlineData("Blog")]
    [InlineData("blog/alt")]
    [InlineData("")]
    public async Task SaveImageAsync_WithInvalidFolder_Throws(string folder)
    {
        // Act
        var act = () => _service.SaveImageAsync(File("a.jpg", Jpeg), folder);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Geçersiz klasör adı.");
    }

    // ---- SavePdfAsync ----

    [Fact]
    public async Task SavePdfAsync_WithValidPdf_Saves()
    {
        // Act
        var path = await _service.SavePdfAsync(File("kvkk.pdf", Pdf), "yasal-metinler");

        // Assert
        path.Should().MatchRegex(@"^/uploads/yasal-metinler/[0-9a-f]{32}\.pdf$");
    }

    [Fact]
    public async Task SavePdfAsync_WithImage_Throws()
    {
        // Act
        var act = () => _service.SavePdfAsync(File("foto.jpg", Jpeg), "yasal-metinler");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    // ---- DeleteAsync ----

    [Fact]
    public async Task DeleteAsync_WithUploadedFile_DeletesIt()
    {
        // Arrange
        var path = await _service.SaveImageAsync(File("a.jpg", Jpeg), "blog");

        // Act
        await _service.DeleteAsync(path);

        // Assert
        System.IO.File.Exists(FullPath(path)).Should().BeFalse();
    }

    [Theory]
    [InlineData("/appsettings.json")]               // uploads dışı
    [InlineData("/uploads/../appsettings.json")]    // üst klasöre çıkma
    [InlineData("/images/logo.png")]                // sitenin kendi görseli
    public async Task DeleteAsync_OutsideUploads_ThrowsAndKeepsFile(string path)
    {
        // Arrange
        var target = FullPath(path.Replace("/uploads/../", "/"));
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        await System.IO.File.WriteAllTextAsync(target, "gizli");

        // Act
        var act = () => _service.DeleteAsync(path);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        System.IO.File.Exists(target).Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_WithMissingFile_DoesNothing()
    {
        // Act
        var act = () => _service.DeleteAsync("/uploads/blog/yok.jpg");

        // Assert
        await act.Should().NotThrowAsync();
    }

    // ---- ListAsync ----

    [Fact]
    public async Task ListAsync_ReturnsOnlyFilesOlderThanGivenDate()
    {
        // Arrange
        var oldPath = await _service.SaveImageAsync(File("eski.jpg", Jpeg), "blog");
        var newPath = await _service.SaveImageAsync(File("yeni.jpg", Jpeg), "blog");
        System.IO.File.SetLastWriteTimeUtc(FullPath(oldPath), DateTime.UtcNow.AddDays(-2));

        // Act
        var paths = await _service.ListAsync(DateTime.UtcNow.AddDays(-1));

        // Assert — yol DB'deki biçimde döner (/uploads/..., ileri eğik çizgi)
        paths.Should().Equal(oldPath);
        paths.Should().NotContain(newPath);
    }
}
