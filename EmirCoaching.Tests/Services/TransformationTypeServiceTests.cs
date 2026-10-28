using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class TransformationTypeServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ITransformationTypeRepository> _types = new();
    private readonly Mock<ITransformationRepository> _transformations = new();
    private readonly TransformationTypeService _service;

    public TransformationTypeServiceTests()
    {
        _uow.Setup(u => u.TransformationTypes).Returns(_types.Object);
        _uow.Setup(u => u.Transformations).Returns(_transformations.Object);
        _types.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new TransformationType { Id = 1 });

        _service = new TransformationTypeService(_uow.Object, TestMapper.Create(),
            new CreateTransformationTypeRequestValidator(), new UpdateTransformationTypeRequestValidator(),
            NullLogger<TransformationTypeService>.Instance);
    }

    [Fact]
    public async Task CreateAsync_WithExistingSlug_ReturnsConflict()
    {
        // Arrange
        _types.Setup(r => r.SlugExistsAsync("kilo-verme", null)).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(new CreateTransformationTypeRequest { Name = "Kilo Verme", Slug = "kilo-verme" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
    }

    [Fact]
    public async Task DeleteAsync_WithTransformationsOfType_ReturnsConflictAndDoesNotRemove()
    {
        // Arrange
        _transformations.Setup(r => r.AnyWithTypeAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        _types.Verify(r => r.Remove(It.IsAny<TransformationType>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithoutTransformations_Removes()
    {
        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _types.Verify(r => r.Remove(It.Is<TransformationType>(t => t.Id == 1)), Times.Once);
    }
}
