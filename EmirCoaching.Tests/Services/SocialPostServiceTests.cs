using AwesomeAssertions;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

// CrudService'in görsel temizliği (ReleaseUploadsAsync) bu servis üzerinden sınanır
public class SocialPostServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ISocialPostRepository> _posts = new();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly SocialPostService _service;

    public SocialPostServiceTests()
    {
        _uow.Setup(u => u.SocialPosts).Returns(_posts.Object);
        _posts.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new SocialPost { Id = 1, ImagePath = "/uploads/video/eski.jpg" });

        _service = new SocialPostService(_uow.Object, TestMapper.Create(),
            new CreateSocialPostRequestValidator(), new UpdateSocialPostRequestValidator(),
            _uploadCleanup.Object, NullLogger<SocialPostService>.Instance);
    }

    [Fact]
    public async Task UpdateAsync_WhenImageChanges_ReleasesOldImage()
    {
        // Act
        var result = await _service.UpdateAsync(Request("/uploads/video/yeni.jpg"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p =>
            p.SequenceEqual(new[] { "/uploads/video/eski.jpg" }))), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenImageUnchanged_ReleasesNothing()
    {
        // Act
        await _service.UpdateAsync(Request("/uploads/video/eski.jpg"));

        // Assert
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p => p.Any())), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReleasesImageAfterSave()
    {
        // Arrange — kayıt sırası: önce SaveChanges, sonra dosya temizliği
        var calls = new List<string>();
        _uow.Setup(u => u.SaveChangesAsync()).Callback(() => calls.Add("save")).ReturnsAsync(1);
        _uploadCleanup.Setup(c => c.DeleteUnusedAsync(It.IsAny<IEnumerable<string>>()))
            .Callback(() => calls.Add("cleanup")).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        calls.Should().Equal("save", "cleanup");
    }

    private static UpdateSocialPostRequest Request(string imagePath) => new()
    {
        Id = 1,
        Platform = SocialPlatform.YouTube,
        Title = "Kardionun Önemi",
        ImagePath = imagePath,
        Url = "https://www.youtube.com/watch?v=abc"
    };
}
