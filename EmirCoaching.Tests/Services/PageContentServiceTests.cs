using AwesomeAssertions;
using EmirCoaching.Business.Common;
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

public class PageContentServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IPageContentRepository> _pages = new();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly PageContentService _service;

    public PageContentServiceTests()
    {
        _uow.Setup(u => u.PageContents).Returns(_pages.Object);
        _service = new PageContentService(_uow.Object, TestMapper.Create(), new UpdatePageContentRequestValidator(),
            _uploadCleanup.Object, NullLogger<PageContentService>.Instance);
    }

    [Fact]
    public async Task UpdateAsync_MarksUntrackedPageAsUpdatedAndReleasesOldImage()
    {
        // Arrange — okuma sorgusu takipsiz (AsNoTracking): değişiklik Update ile işaretlenmeli
        var page = new PageContent { Id = 1, PageKey = "hakkimda", BackgroundImagePath = "/uploads/sayfa/eski.jpg" };
        _pages.Setup(r => r.GetByPageKeyAsync("hakkimda")).ReturnsAsync(page);

        // Act
        var result = await _service.UpdateAsync("hakkimda", new UpdatePageContentRequest
        {
            Title = "Hakkımda", BackgroundImagePath = "/uploads/sayfa/yeni.jpg"
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        _pages.Verify(r => r.Update(page), Times.Once);
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p =>
            p.SequenceEqual(new[] { "/uploads/sayfa/eski.jpg" }))), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithUnknownPageKey_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateAsync("yok", new UpdatePageContentRequest { Title = "Başlık" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    [Fact]
    public async Task GetAllAsync_OrdersByPageKey()
    {
        // Arrange
        _pages.Setup(r => r.GetAllAsync()).ReturnsAsync(
            [new PageContent { PageKey = "paketler" }, new PageContent { PageKey = "blog" }, new PageContent { PageKey = "hakkimda" }]);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Value!.Select(p => p.PageKey).Should().Equal("blog", "hakkimda", "paketler");
    }
}
