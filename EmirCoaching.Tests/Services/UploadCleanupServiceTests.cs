using AwesomeAssertions;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class UploadCleanupServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IUploadReferenceRepository> _references = new();
    private readonly Mock<IFileStorageService> _storage = new();
    private readonly UploadCleanupService _service;

    public UploadCleanupServiceTests()
    {
        _uow.Setup(u => u.UploadReferences).Returns(_references.Object);
        _service = new UploadCleanupService(_uow.Object, _storage.Object, NullLogger<UploadCleanupService>.Instance);
    }

    private void SetUsedPaths(params string[] paths) =>
        _references.Setup(r => r.GetUsedPathsAsync()).ReturnsAsync(paths.ToHashSet(StringComparer.OrdinalIgnoreCase));

    [Fact]
    public async Task DeleteUnusedAsync_DeletesOnlyPathsNotUsedByAnyRecord()
    {
        // Arrange — b.jpg başka bir kayıtta hâlâ kullanılıyor
        SetUsedPaths("/uploads/b.jpg");

        // Act
        await _service.DeleteUnusedAsync(["/uploads/a.jpg", "/uploads/b.jpg"]);

        // Assert
        _storage.Verify(s => s.DeleteAsync("/uploads/a.jpg"), Times.Once);
        _storage.Verify(s => s.DeleteAsync("/uploads/b.jpg"), Times.Never);
    }

    [Fact]
    public async Task DeleteUnusedAsync_WithDuplicatePaths_DeletesOnce()
    {
        // Arrange
        SetUsedPaths();

        // Act
        await _service.DeleteUnusedAsync(["/uploads/a.jpg", "/Uploads/A.jpg"]);

        // Assert
        _storage.Verify(s => s.DeleteAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task DeleteUnusedAsync_WithNoPaths_DoesNotQueryDatabase()
    {
        // Act
        await _service.DeleteUnusedAsync([]);

        // Assert
        _references.Verify(r => r.GetUsedPathsAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteOrphansAsync_DeletesOldUnusedFilesAndReturnsCount()
    {
        // Arrange — panelde yüklenip hiçbir kayda yazılmamış eski dosyalar
        var before = new DateTime(2026, 10, 27);
        _storage.Setup(s => s.ListAsync(before)).ReturnsAsync(["/uploads/kullanilan.jpg", "/uploads/sahipsiz.jpg"]);
        SetUsedPaths("/uploads/kullanilan.jpg");

        // Act
        var deleted = await _service.DeleteOrphansAsync(before);

        // Assert
        deleted.Should().Be(1);
        _storage.Verify(s => s.DeleteAsync("/uploads/sahipsiz.jpg"), Times.Once);
        _storage.Verify(s => s.DeleteAsync("/uploads/kullanilan.jpg"), Times.Never);
    }
}
