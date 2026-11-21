using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
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

public class DietFormSubmissionServiceTests
{
    private const int ActivePackageId = 4;
    private static readonly RequestInfo Info = new("10.0.0.1", "Test tarayıcı");

    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IDietFormSubmissionRepository> _submissions = new();
    private readonly Mock<IPackageRepository> _packages = new();
    private readonly Mock<IConsentRecordService> _consents = new();
    private readonly DietFormSubmissionService _service;

    public DietFormSubmissionServiceTests()
    {
        _uow.Setup(u => u.DietFormSubmissions).Returns(_submissions.Object);
        _uow.Setup(u => u.Packages).Returns(_packages.Object);
        _packages.Setup(r => r.GetByIdAsync(ActivePackageId)).ReturnsAsync(new Package { Id = ActivePackageId, IsActive = true });
        _service = new DietFormSubmissionService(_uow.Object, TestMapper.Create(), _consents.Object,
            new CreateDietFormSubmissionRequestValidator(), new UpdateDietFormSubmissionClientFormRequestValidator(),
            NullLogger<DietFormSubmissionService>.Instance);
    }

    // ---- SubmitAsync ----

    [Fact]
    public async Task SubmitAsync_WithValidRequest_SavesAsNewWithNormalizedContact()
    {
        // Arrange
        DietFormSubmission? added = null;
        _submissions.Setup(r => r.AddAsync(It.IsAny<DietFormSubmission>())).Callback<DietFormSubmission>(s => added = s);

        // Act
        var result = await _service.SubmitAsync(ValidRequest() with { Email = " Ayse@Example.com ", PhoneNumber = "+90 555 111 22 33" }, Info);

        // Assert — aynı kişinin farklı yazımları mükerrer kontrolünde tek biçime iner
        result.IsSuccess.Should().BeTrue();
        added!.Status.Should().Be(DietFormStatus.Yeni);
        added.PackageId.Should().Be(ActivePackageId);
        added.Email.Should().Be("ayse@example.com");
        added.PhoneNumber.Should().Be("05551112233");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_WithHealthDataConsent_RecordsBothConsents()
    {
        // Act
        await _service.SubmitAsync(ValidRequest() with { HealthDataConsent = true }, Info);

        // Assert
        _consents.Verify(c => c.RecordAsync(It.Is<ConsentRecord>(r => r.ConsentType == ConsentType.Gizlilik && r.IsGranted)), Times.Once);
        _consents.Verify(c => c.RecordAsync(It.Is<ConsentRecord>(r => r.ConsentType == ConsentType.AcikRizaSaglik && r.IsGranted)), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_WithoutHealthDataConsent_RecordsOnlyPrivacyConsent()
    {
        // Act
        await _service.SubmitAsync(ValidRequest(), Info);

        // Assert
        _consents.Verify(c => c.RecordAsync(It.IsAny<ConsentRecord>()), Times.Once);
        _consents.Verify(c => c.RecordAsync(It.Is<ConsentRecord>(r => r.ConsentType == ConsentType.Gizlilik)), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_WithPendingSubmission_ReturnsConflictAndDoesNotSave()
    {
        // Arrange — aynı kişinin değerlendirilmeyi bekleyen başvurusu var
        _submissions.Setup(r => r.HasPendingAsync("ayse@example.com", "05551112233")).ReturnsAsync(true);

        // Act
        var result = await _service.SubmitAsync(ValidRequest(), Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        _submissions.Verify(r => r.AddAsync(It.IsAny<DietFormSubmission>()), Times.Never);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Theory]
    [InlineData(99)] // yok
    [InlineData(5)]  // pasif
    public async Task SubmitAsync_WithMissingOrInactivePackage_ReturnsValidationErrorAndDoesNotSave(int packageId)
    {
        // Arrange
        _packages.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(new Package { Id = 5, IsActive = false });

        // Act
        var result = await _service.SubmitAsync(ValidRequest() with { PackageId = packageId }, Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SubmitAsync_WithoutPrivacyAccepted_ReturnsValidationError()
    {
        // Act
        var result = await _service.SubmitAsync(ValidRequest() with { PrivacyAccepted = false }, Info);

        // Assert
        result.Errors.Should().ContainSingle().Which.Should().Be("Gizlilik Politikası onayı zorunludur.");
        _consents.Verify(c => c.RecordAsync(It.IsAny<ConsentRecord>()), Times.Never);
    }

    // ---- UpdateStatusAsync / DeleteAsync ----

    [Fact]
    public async Task UpdateStatusAsync_WithExistingId_ChangesStatus()
    {
        // Arrange
        var submission = new DietFormSubmission { Id = 1, Status = DietFormStatus.Yeni };
        _submissions.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(submission);
        var newStatus = Enum.GetValues<DietFormStatus>().Last();

        // Act
        var result = await _service.UpdateStatusAsync(1, newStatus);

        // Assert
        result.IsSuccess.Should().BeTrue();
        submission.Status.Should().Be(newStatus);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithUndefinedStatus_ReturnsValidationError()
    {
        // Act
        var result = await _service.UpdateStatusAsync(1, (DietFormStatus)99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _submissions.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateStatusAsync(99, DietFormStatus.Yeni);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    // ---- UpdateClientFormAsync ----

    [Fact]
    public async Task UpdateClientFormAsync_WithGoogleFormUrl_SavesLink()
    {
        // Arrange
        var submission = new DietFormSubmission { Id = 1 };
        _submissions.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(submission);

        // Act
        var result = await _service.UpdateClientFormAsync(1, new() { ClientFormUrl = " https://forms.gle/AbC123 " });

        // Assert
        result.IsSuccess.Should().BeTrue();
        submission.ClientFormUrl.Should().Be("https://forms.gle/AbC123");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("  ")]
    public async Task UpdateClientFormAsync_WithEmptyUrl_RemovesLink(string? url)
    {
        // Arrange
        var submission = new DietFormSubmission { Id = 1, ClientFormUrl = "https://forms.gle/AbC123" };
        _submissions.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(submission);

        // Act
        var result = await _service.UpdateClientFormAsync(1, new() { ClientFormUrl = url });

        // Assert
        result.IsSuccess.Should().BeTrue();
        submission.ClientFormUrl.Should().BeNull();
    }

    [Fact]
    public async Task UpdateClientFormAsync_WithNonGoogleUrl_ReturnsValidationErrorAndDoesNotSave()
    {
        // Act
        var result = await _service.UpdateClientFormAsync(1, new() { ClientFormUrl = "https://example.com/form" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateClientFormAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateClientFormAsync(99, new() { ClientFormUrl = "https://forms.gle/AbC123" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingId_DeletesPermanently()
    {
        // Arrange
        _submissions.Setup(r => r.DeletePermanentlyAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _submissions.Verify(r => r.Remove(It.IsAny<DietFormSubmission>()), Times.Never);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenDeleteFails_RollsBackAndRethrows()
    {
        // Arrange
        _submissions.Setup(r => r.DeletePermanentlyAsync(1)).ThrowsAsync(new InvalidOperationException());

        // Act
        var act = () => _service.DeleteAsync(1);

        // Assert — onay kayıtları silinip başvuru kalmasın: iki DELETE tek transaction'da
        await act.Should().ThrowAsync<InvalidOperationException>();
        _uow.Verify(u => u.RollbackAsync(), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteManyAsync_WhenOneIsMissing_RollsBackAll()
    {
        // Arrange — 1 silindi, 2 yok: ilk silme de geri alınmalı
        _submissions.Setup(r => r.DeletePermanentlyAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteManyAsync([1, 2]);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        _uow.Verify(u => u.RollbackAsync(), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteManyAsync_WithExistingIds_DeletesAllInOneTransaction()
    {
        // Arrange
        _submissions.Setup(r => r.DeletePermanentlyAsync(It.IsAny<int>())).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteManyAsync([1, 2, 2]);

        // Assert — tekrar eden numara bir kez silinir
        result.IsSuccess.Should().BeTrue();
        _submissions.Verify(r => r.DeletePermanentlyAsync(2), Times.Once);
        _uow.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    private static CreateDietFormSubmissionRequest ValidRequest() => new()
    {
        PackageId = ActivePackageId,
        FullName = "Ayşe Yılmaz",
        Age = 28,
        Email = "ayse@example.com",
        PhoneNumber = "0555 111 22 33",
        HeightCm = 165,
        CurrentWeight = 70,
        TargetWeight = 60,
        PrivacyAccepted = true
    };
}
