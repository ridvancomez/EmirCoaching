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

public class DietProgramServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IDietProgramRepository> _programs = new();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly DietProgramService _service;

    public DietProgramServiceTests()
    {
        _uow.Setup(u => u.DietPrograms).Returns(_programs.Object);
        _service = new DietProgramService(_uow.Object, TestMapper.Create(), new UpdateDietProgramRequestValidator(),
            _uploadCleanup.Object, NullLogger<DietProgramService>.Instance);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesSeededRecordAndReleasesOldPhoto()
    {
        // Arrange — tek kayıt (Id 1) seed ile gelir
        var program = new DietProgram { Id = 1, DietitianPhotoPath = "/uploads/diyet/eski.jpg" };
        _programs.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(program);

        // Act
        var result = await _service.UpdateAsync(Request() with { DietitianPhotoPath = "/uploads/diyet/yeni.jpg" });

        // Assert
        result.IsSuccess.Should().BeTrue();
        program.Price.Should().Be(1500);
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p =>
            p.SequenceEqual(new[] { "/uploads/diyet/eski.jpg" }))), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithoutSeedRecord_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateAsync(Request());

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    [Fact]
    public async Task GetActiveAsync_WhenProgramInactive_ReturnsNotFound()
    {
        // Act — repository yalnızca aktif programı döner
        var result = await _service.GetActiveAsync();

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    private static UpdateDietProgramRequest Request() => new()
    {
        Title = "Kişiye Özel Diyet",
        Price = 1500,
        DurationWeeks = 4,
        DietitianTitle = "Dyt.",
        DietitianFullName = "Ayşe Demir",
        IsActive = true
    };
}
