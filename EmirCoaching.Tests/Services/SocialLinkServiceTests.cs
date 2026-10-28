using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
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

public class SocialLinkServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ISocialLinkRepository> _links = new();
    private readonly SocialLinkService _service;

    public SocialLinkServiceTests()
    {
        _uow.Setup(u => u.SocialLinks).Returns(_links.Object);
        _service = new SocialLinkService(_uow.Object, TestMapper.Create(),
            new CreateSocialLinkRequestValidator(), new UpdateSocialLinkRequestValidator(),
            NullLogger<SocialLinkService>.Instance);
    }

    [Fact]
    public async Task CreateAsync_WithPlatformAlreadyLinked_ReturnsConflict()
    {
        // Arrange — her platform için tek link
        _links.Setup(r => r.PlatformExistsAsync(SocialPlatform.Instagram, null)).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(Request());

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        _links.Verify(r => r.AddAsync(It.IsAny<SocialLink>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ChecksPlatformExcludingItself()
    {
        // Arrange
        _links.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(new SocialLink { Id = 3, Platform = SocialPlatform.Instagram });

        // Act
        var result = await _service.UpdateAsync(new UpdateSocialLinkRequest
        {
            Id = 3, Platform = SocialPlatform.Instagram, Url = "https://instagram.com/emir", Description = "Instagram"
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        _links.Verify(r => r.PlatformExistsAsync(SocialPlatform.Instagram, 3), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithHttpUrl_ReturnsValidationError()
    {
        // Act
        var result = await _service.CreateAsync(Request() with { Url = "http://instagram.com/emir" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    private static CreateSocialLinkRequest Request() => new()
    {
        Platform = SocialPlatform.Instagram,
        Url = "https://instagram.com/emir",
        Description = "Instagram"
    };
}
