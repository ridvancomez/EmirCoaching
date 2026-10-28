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

public class SiteSettingServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ISiteSettingRepository> _settings = new();
    private readonly SiteSettingService _service;

    public SiteSettingServiceTests()
    {
        _uow.Setup(u => u.SiteSettings).Returns(_settings.Object);
        _service = new SiteSettingService(_uow.Object, TestMapper.Create(), new UpdateSiteSettingRequestValidator(),
            NullLogger<SiteSettingService>.Instance);
    }

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesSeededRecord()
    {
        // Arrange
        var setting = new SiteSetting { Id = 1 };
        _settings.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(setting);

        // Act
        var result = await _service.UpdateAsync(Request());

        // Assert
        result.IsSuccess.Should().BeTrue();
        setting.Email.Should().Be("info@emircoaching.com");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidEmail_ReturnsValidationError()
    {
        // Act
        var result = await _service.UpdateAsync(Request() with { Email = "gecersiz" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    private static UpdateSiteSettingRequest Request() => new()
    {
        Phone = "0555 111 22 33",
        Email = "info@emircoaching.com",
        LegalEmail = "kvkk@emircoaching.com",
        Address = "İstanbul",
        WorkingDays1 = "Pzt - Cuma",
        WorkingHours1 = "09:00 - 18:00",
        WorkingDays2 = "Cumartesi",
        WorkingHours2 = "10:00 - 14:00"
    };
}
