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

public class TransformationServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ITransformationRepository> _transformations = new();
    private readonly Mock<ITransformationTypeRepository> _types = new();
    private readonly Mock<ITransformationMetricRepository> _metrics = new();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly TransformationService _service;

    public TransformationServiceTests()
    {
        _uow.Setup(u => u.Transformations).Returns(_transformations.Object);
        _uow.Setup(u => u.TransformationTypes).Returns(_types.Object);
        _uow.Setup(u => u.TransformationMetrics).Returns(_metrics.Object);
        _types.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new TransformationType { Id = 1 });

        _service = new TransformationService(_uow.Object, TestMapper.Create(),
            new CreateTransformationRequestValidator(), new UpdateTransformationRequestValidator(),
            _uploadCleanup.Object, NullLogger<TransformationService>.Instance);
    }

    // ---- Okuma ----

    [Fact]
    public async Task GetDetailAsync_WithInactiveTransformation_ReturnsNotFound()
    {
        // Arrange — pasif değişim sitede hiçbir yerde görünmez
        var transformation = ExistingTransformation();
        transformation.IsActive = false;
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(transformation);

        // Act
        var result = await _service.GetDetailAsync(1);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    [Fact]
    public async Task GetByIdAsync_WithInactiveTransformation_ReturnsItForAdmin()
    {
        // Arrange
        var transformation = ExistingTransformation();
        transformation.IsActive = false;
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(transformation);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetDetailAsync_ReturnsMetricsInSortOrder()
    {
        // Arrange — ölçümler sırasız yüklenir, servis SortOrder'a göre dizer
        var transformation = ExistingTransformation();
        transformation.Metrics.Single(m => m.Id == 10).SortOrder = 2;
        transformation.Metrics.Single(m => m.Id == 20).SortOrder = 1;
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(transformation);

        // Act
        var result = await _service.GetDetailAsync(1);

        // Assert
        result.Value!.Metrics.Select(m => m.Id).Should().Equal(20, 10);
    }

    [Fact]
    public async Task GetDetailAsync_ReturnsUploadedImagesAsFullUrl()
    {
        // Arrange
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(ExistingTransformation());

        // Act
        var result = await _service.GetDetailAsync(1);

        // Assert — DB'de göreli yol, yanıtta API adresiyle tam URL
        result.Value!.BeforeImageUrl.Should().Be(TestMapper.PublicBaseUrl + "/uploads/degisim/once.jpg");
    }

    // ---- CreateAsync ----

    [Fact]
    public async Task CreateAsync_WithValidRequest_AddsTransformationWithMetrics()
    {
        // Arrange
        Transformation? added = null;
        _transformations.Setup(r => r.AddAsync(It.IsAny<Transformation>())).Callback<Transformation>(t => added = t);

        // Act
        var result = await _service.CreateAsync(ValidCreateRequest());

        // Assert
        result.IsSuccess.Should().BeTrue();
        added!.Metrics.Select(m => (m.Label, m.SortOrder)).Should().Equal(("Kilo", 1), ("Bel", 2));
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithMissingType_ReturnsValidationError()
    {
        // Act
        var result = await _service.CreateAsync(ValidCreateRequest() with { TransformationTypeId = 9 });

        // Assert
        result.Errors.Should().Contain("Dönüşüm tipi bulunamadı.");
        _transformations.Verify(r => r.AddAsync(It.IsAny<Transformation>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithFullUploadUrlFromAnotherSite_ReturnsValidationError()
    {
        // Act — StoredPathJson yalnızca kendi API adresini kırpar; başka adres SitePath'e takılır
        var result = await _service.CreateAsync(ValidCreateRequest() with { BeforeImagePath = "https://baska.site/a.jpg" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    // ---- UpdateAsync ----

    [Fact]
    public async Task UpdateAsync_SyncsMetricsById()
    {
        // Arrange — 10 güncellenir, 20 silinir, Id'siz olan eklenir
        var transformation = ExistingTransformation();
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(transformation);
        var request = ValidUpdateRequest() with
        {
            Metrics = [new TransformationMetricRequest { Id = 10, Label = "Kilo", Value = "-12 kg" }, new TransformationMetricRequest { Label = "Yağ", Value = "-%8" }]
        };

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        transformation.Metrics.Should().Contain(m => m.Id == 10 && m.Value == "-12 kg" && m.SortOrder == 1);
        transformation.Metrics.Should().Contain(m => m.Id == 0 && m.Label == "Yağ" && m.SortOrder == 2);
        _metrics.Verify(r => r.Remove(It.Is<TransformationMetric>(m => m.Id == 20)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithMetricOfAnotherTransformation_ReturnsValidationErrorAndDoesNotSave()
    {
        // Arrange
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(ExistingTransformation());
        var request = ValidUpdateRequest() with { Metrics = [new TransformationMetricRequest { Id = 99, Label = "Kilo", Value = "-5 kg" }] };

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenImagesChange_DeletesOnlyReplacedImages()
    {
        // Arrange
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(ExistingTransformation());

        // Act — "önce" fotoğrafı değişti, "sonra" aynı kaldı
        await _service.UpdateAsync(ValidUpdateRequest() with { BeforeImagePath = "/uploads/degisim/yeni-once.jpg" });

        // Assert
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p =>
            p.SequenceEqual(new[] { "/uploads/degisim/once.jpg" }))), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateAsync(ValidUpdateRequest());

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    // ---- DeleteAsync ----

    [Fact]
    public async Task DeleteAsync_WithExistingId_RemovesAndReleasesImages()
    {
        // Arrange
        var transformation = ExistingTransformation();
        _transformations.Setup(r => r.GetWithMetricsAsync(1)).ReturnsAsync(transformation);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _transformations.Verify(r => r.Remove(transformation), Times.Once);
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p =>
            p.Contains("/uploads/degisim/once.jpg") && p.Contains("/uploads/degisim/sonra.jpg"))), Times.Once);
    }

    // ---- Test verisi ----

    private static Transformation ExistingTransformation() => new()
    {
        Id = 1,
        ClientName = "Can",
        IsActive = true,
        TransformationTypeId = 1,
        TransformationType = new TransformationType { Id = 1, Name = "Kilo Verme", Slug = "kilo-verme" },
        BeforeImagePath = "/uploads/degisim/once.jpg",
        AfterImagePath = "/uploads/degisim/sonra.jpg",
        Metrics =
        [
            new TransformationMetric { Id = 10, Label = "Kilo", Value = "-10 kg", SortOrder = 1 },
            new TransformationMetric { Id = 20, Label = "Bel", Value = "-8 cm", SortOrder = 2 }
        ]
    };

    private static CreateTransformationRequest ValidCreateRequest() => new()
    {
        ClientName = "Can",
        Age = 30,
        DurationWeeks = 12,
        TransformationTypeId = 1,
        Quote = "Hayatım değişti.",
        Story = "Uzun hikâye",
        BeforeImagePath = "/uploads/degisim/once.jpg",
        AfterImagePath = "/uploads/degisim/sonra.jpg",
        Year = 2026,
        IsActive = true,
        Metrics = [new TransformationMetricRequest { Label = "Kilo", Value = "-10 kg" }, new TransformationMetricRequest { Label = "Bel", Value = "-8 cm" }]
    };

    private static UpdateTransformationRequest ValidUpdateRequest() => new()
    {
        Id = 1,
        ClientName = "Can",
        Age = 30,
        DurationWeeks = 12,
        TransformationTypeId = 1,
        Quote = "Hayatım değişti.",
        Story = "Uzun hikâye",
        BeforeImagePath = "/uploads/degisim/once.jpg",
        AfterImagePath = "/uploads/degisim/sonra.jpg",
        Year = 2026,
        IsActive = true,
        Metrics = [new TransformationMetricRequest { Id = 10, Label = "Kilo", Value = "-10 kg" }, new TransformationMetricRequest { Id = 20, Label = "Bel", Value = "-8 cm" }]
    };
}
