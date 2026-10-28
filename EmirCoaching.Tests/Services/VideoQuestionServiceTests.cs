using AwesomeAssertions;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

// CrudService'in görsel temizliği (ReleaseUploadsAsync) bu servis üzerinden sınanır
public class VideoQuestionServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IVideoQuestionRepository> _questions = new();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly VideoQuestionService _service;

    public VideoQuestionServiceTests()
    {
        _uow.Setup(u => u.VideoQuestions).Returns(_questions.Object);
        _questions.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new VideoQuestion { Id = 1, ThumbnailPath = "/uploads/video/eski.jpg" });

        _service = new VideoQuestionService(_uow.Object, TestMapper.Create(),
            new CreateVideoQuestionRequestValidator(), new UpdateVideoQuestionRequestValidator(),
            _uploadCleanup.Object, NullLogger<VideoQuestionService>.Instance);
    }

    [Fact]
    public async Task UpdateAsync_WhenThumbnailChanges_ReleasesOldThumbnail()
    {
        // Act
        var result = await _service.UpdateAsync(Request("/uploads/video/yeni.jpg"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p =>
            p.SequenceEqual(new[] { "/uploads/video/eski.jpg" }))), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenThumbnailUnchanged_ReleasesNothing()
    {
        // Act
        await _service.UpdateAsync(Request("/uploads/video/eski.jpg"));

        // Assert
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p => p.Any())), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_ReleasesThumbnailAfterSave()
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

    private static UpdateVideoQuestionRequest Request(string thumbnailPath) => new()
    {
        Id = 1,
        Question = "Kaç gün antrenman?",
        Description = "Açıklama",
        ThumbnailPath = thumbnailPath,
        YouTubeUrl = "https://www.youtube.com/watch?v=abc"
    };
}
