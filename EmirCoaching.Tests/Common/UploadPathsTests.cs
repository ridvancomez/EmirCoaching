using AwesomeAssertions;
using EmirCoaching.Business.Common;

namespace EmirCoaching.Tests.Common;

public class UploadPathsTests
{
    private class Sample
    {
        public string CoverImagePath { get; set; } = "";
        public string? ThumbnailPath { get; set; }
        public string SiteImagePath { get; set; } = "";
        public string Title { get; set; } = "";
    }

    [Fact]
    public void Of_ReturnsOnlyUploadPathProperties()
    {
        // Arrange
        var entity = new Sample
        {
            CoverImagePath = "/uploads/blog/a.jpg",
            ThumbnailPath = "/Uploads/blog/b.jpg",   // büyük/küçük harf fark etmez
            SiteImagePath = "/images/logo.png",       // sitenin kendi görseli: silinmez
            Title = "/uploads/aslinda-baslik"         // adı *Path değil
        };

        // Act
        var paths = UploadPaths.Of(entity);

        // Assert
        paths.Should().BeEquivalentTo(["/uploads/blog/a.jpg", "/Uploads/blog/b.jpg"]);
    }

    [Fact]
    public void Of_WithEmptyOrNullPaths_ReturnsEmpty()
    {
        UploadPaths.Of(new Sample()).Should().BeEmpty();
    }
}
