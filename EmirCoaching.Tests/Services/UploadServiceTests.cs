using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace EmirCoaching.Tests.Services;

public class UploadServiceTests
{
    private readonly Mock<IFileStorageService> _storage = new();
    private readonly IFormFile _file = Mock.Of<IFormFile>();
    private readonly UploadService _service;

    public UploadServiceTests()
    {
        _service = new UploadService(_storage.Object,
            Options.Create(new StorageOptions { PublicBaseUrl = "https://api.test" }), NullLogger<UploadService>.Instance);
    }

    [Fact]
    public async Task UploadImageAsync_ReturnsStoredPathAndPublicUrl()
    {
        // Arrange
        _storage.Setup(s => s.SaveImageAsync(_file, "blog")).ReturnsAsync("/uploads/blog/a.jpg");

        // Act
        var result = await _service.UploadImageAsync(_file, " blog ");

        // Assert — panel path'i kayda yazar, url'yi önizlemede gösterir
        result.Value!.Path.Should().Be("/uploads/blog/a.jpg");
        result.Value.Url.Should().Be("https://api.test/uploads/blog/a.jpg");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task UploadImageAsync_WithBlankFolder_UsesDefaultFolder(string? folder)
    {
        // Arrange
        _storage.Setup(s => s.SaveImageAsync(_file, null)).ReturnsAsync("/uploads/images/a.jpg");

        // Act
        await _service.UploadImageAsync(_file, folder);

        // Assert — null verilince FileStorageService varsayılan "images" klasörünü seçer
        _storage.Verify(s => s.SaveImageAsync(_file, null), Times.Once);
    }

    [Fact]
    public async Task UploadImageAsync_WithRejectedFile_ReturnsValidationError()
    {
        // Arrange
        _storage.Setup(s => s.SaveImageAsync(_file, It.IsAny<string?>())).ThrowsAsync(new ArgumentException("Dosya boş."));

        // Act
        var result = await _service.UploadImageAsync(_file, "blog");

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        result.Errors.Should().Equal("Dosya boş.");
    }
}
