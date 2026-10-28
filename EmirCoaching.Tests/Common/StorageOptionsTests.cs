using AwesomeAssertions;
using EmirCoaching.Business.Common;

namespace EmirCoaching.Tests.Common;

public class StorageOptionsTests
{
    private readonly StorageOptions _options = new() { PublicBaseUrl = "https://api.test/" };

    // ---- ToPublicUrl ----

    [Fact]
    public void ToPublicUrl_WithUploadPath_PrefixesApiAddress()
    {
        _options.ToPublicUrl("/uploads/blog/a.jpg").Should().Be("https://api.test/uploads/blog/a.jpg");
    }

    [Fact]
    public void ToPublicUrl_WithSiteImagePath_KeepsRelativePath()
    {
        _options.ToPublicUrl("/images/logo.png").Should().Be("/images/logo.png");
    }

    [Fact]
    public void ToPublicUrl_WithoutPublicBaseUrl_KeepsRelativePath()
    {
        new StorageOptions().ToPublicUrl("/uploads/a.jpg").Should().Be("/uploads/a.jpg");
    }

    // ---- ToStoredPath ----

    [Theory]
    [InlineData("https://api.test/uploads/blog/a.jpg")]
    [InlineData("HTTPS://API.TEST/uploads/blog/a.jpg")]
    public void ToStoredPath_WithOwnUploadUrl_ReturnsRelativePath(string url)
    {
        _options.ToStoredPath(url).Should().Be("/uploads/blog/a.jpg");
    }

    [Theory]
    [InlineData("/uploads/blog/a.jpg")]                 // zaten göreli
    [InlineData("/images/logo.png")]                    // sitenin görseli
    [InlineData("https://baska.site/uploads/a.jpg")]    // başka alan adı: doğrulamada reddedilecek
    [InlineData("https://api.test/images/logo.png")]    // API adresi ama uploads dışı
    [InlineData(null)]
    public void ToStoredPath_WithOtherValues_ReturnsValueUnchanged(string? value)
    {
        _options.ToStoredPath(value).Should().Be(value);
    }

    [Fact]
    public void ToStoredPath_RoundTripsToPublicUrl()
    {
        // Panel GET'te aldığı URL'yi PUT'ta geri gönderir; kayıtta yine aynı yol durmalı
        const string path = "/uploads/packages/b.webp";

        _options.ToStoredPath(_options.ToPublicUrl(path)).Should().Be(path);
    }
}
