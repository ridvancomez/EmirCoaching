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

public class CertificateServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ICertificateRepository> _certificates = new();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly CertificateService _service;

    public CertificateServiceTests()
    {
        _uow.Setup(u => u.Certificates).Returns(_certificates.Object);
        _service = new CertificateService(_uow.Object, TestMapper.Create(),
            new CreateCertificateRequestValidator(), new UpdateCertificateRequestValidator(),
            _uploadCleanup.Object, NullLogger<CertificateService>.Instance);
    }

    [Fact]
    public async Task GetActiveAsync_ReturnsSiteLogoAsRelativeAndUploadAsFullUrl()
    {
        // Arrange — seed logoları sitenin kendi görseli, panelden yüklenenler API adresini alır
        _certificates.Setup(r => r.GetActiveOrderedAsync()).ReturnsAsync(
        [
            new Certificate { Id = 1, LogoPath = "/images/sertifika-fmi.png" },
            new Certificate { Id = 2, LogoPath = "/uploads/sertifika/yeni.png" }
        ]);

        // Act
        var result = await _service.GetActiveAsync();

        // Assert
        result.Value!.Select(c => c.LogoUrl).Should().Equal(
            "/images/sertifika-fmi.png", $"{TestMapper.PublicBaseUrl}/uploads/sertifika/yeni.png");
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_Saves()
    {
        // Act
        var result = await _service.CreateAsync(Request());

        // Assert
        result.IsSuccess.Should().BeTrue();
        _certificates.Verify(r => r.AddAsync(It.Is<Certificate>(c => c.Issuer == "FMI" && c.Detail == "EREPS Seviye 4")), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithFutureYear_ReturnsValidationErrorAndDoesNotSave()
    {
        // Act
        var result = await _service.CreateAsync(Request() with { Year = DateTime.UtcNow.Year + 1 });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenLogoChanges_ReleasesOldLogo()
    {
        // Arrange
        _certificates.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Certificate { Id = 1, LogoPath = "/uploads/sertifika/eski.png" });
        var request = Request() with { LogoPath = "/uploads/sertifika/yeni.png" };

        // Act
        var result = await _service.UpdateAsync(new UpdateCertificateRequest
        {
            Id = 1, Title = request.Title, Issuer = request.Issuer, IssuerFullName = request.IssuerFullName,
            Detail = request.Detail, Year = request.Year, LogoPath = request.LogoPath, IsActive = true
        });

        // Assert
        result.IsSuccess.Should().BeTrue();
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p =>
            p.SequenceEqual(new[] { "/uploads/sertifika/eski.png" }))), Times.Once);
    }

    private static CreateCertificateRequest Request() => new()
    {
        Title = "Nefes, Nörolojik Düzenleme ve Postür Eğitimi",
        Issuer = "FMI",
        IssuerFullName = "Fitness Movement Institute",
        Detail = "EREPS Seviye 4",
        Year = 2022,
        LogoPath = "/images/sertifika-fmi.png",
        IsActive = true
    };
}
