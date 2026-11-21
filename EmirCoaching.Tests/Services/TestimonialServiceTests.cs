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

public class TestimonialServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ITestimonialRepository> _testimonials = new();
    private readonly Mock<IPackageRepository> _packages = new();
    private readonly TestimonialService _service;

    public TestimonialServiceTests()
    {
        _uow.Setup(u => u.Testimonials).Returns(_testimonials.Object);
        _uow.Setup(u => u.Packages).Returns(_packages.Object);
        _service = new TestimonialService(_uow.Object, TestMapper.Create(),
            new CreateTestimonialRequestValidator(), new UpdateTestimonialRequestValidator(),
            NullLogger<TestimonialService>.Instance);
    }

    [Fact]
    public async Task GetHomeAsync_ReturnsPackageNameAndDuration()
    {
        // Arrange — carousel'de "A.K. · Dönüşüm Protokolü, 6 ay"
        var package = new Package { Id = 2, Name = "Dönüşüm Protokolü", DurationMonths = 6 };
        _testimonials.Setup(r => r.GetHomeAsync())
            .ReturnsAsync([new Testimonial { Id = 1, Name = "A.K.", ShowOnHome = true, PackageId = 2, Package = package }]);

        // Act
        var result = await _service.GetHomeAsync();

        // Assert
        var testimonial = result.Value!.Should().ContainSingle().Subject;
        testimonial.PackageName.Should().Be("Dönüşüm Protokolü");
        testimonial.PackageDurationMonths.Should().Be(6);
    }

    [Fact]
    public async Task CreateAsync_WithExistingPackage_Saves()
    {
        // Arrange
        _packages.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(new Package { Id = 2 });

        // Act
        var result = await _service.CreateAsync(Request(packageId: 2));

        // Assert
        result.IsSuccess.Should().BeTrue();
        _testimonials.Verify(r => r.AddAsync(It.Is<Testimonial>(t => t.PackageId == 2 && t.Rating == 4.5m)), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithMissingPackage_ReturnsValidationError()
    {
        // Act — yorum pakete zorunlu bağlı
        var result = await _service.CreateAsync(Request(packageId: 9));

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        result.Errors.Should().Contain("Seçilen paket bulunamadı.");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithoutPackage_ReturnsValidationError()
    {
        // Act
        var result = await _service.CreateAsync(Request(packageId: 0));

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Fact]
    public async Task GetAllAsync_WithPackageFilter_QueriesOnlyThatPackage()
    {
        // Act
        await _service.GetAllAsync(packageId: 2);

        // Assert
        _testimonials.Verify(r => r.GetOrderedAsync(2), Times.Once);
    }

    private static CreateTestimonialRequest Request(int packageId) => new()
    {
        Name = "Mert",
        Text = "Harika bir program.",
        Rating = 4.5m,
        PackageId = packageId
    };
}
