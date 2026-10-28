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

public class ContactMessageServiceTests
{
    private static readonly RequestInfo Info = new("10.0.0.1", "Test tarayıcı");

    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IContactMessageRepository> _messages = new();
    private readonly Mock<IPackageRepository> _packages = new();
    private readonly Mock<IConsentRecordService> _consents = new();
    private readonly ContactMessageService _service;

    public ContactMessageServiceTests()
    {
        _uow.Setup(u => u.ContactMessages).Returns(_messages.Object);
        _uow.Setup(u => u.Packages).Returns(_packages.Object);
        _service = new ContactMessageService(_uow.Object, TestMapper.Create(), _consents.Object,
            new CreateContactMessageRequestValidator(), NullLogger<ContactMessageService>.Instance);
    }

    [Fact]
    public async Task SubmitAsync_WithValidRequest_SavesMessageWithPrivacyConsent()
    {
        // Arrange
        ContactMessage? added = null;
        _messages.Setup(r => r.AddAsync(It.IsAny<ContactMessage>())).Callback<ContactMessage>(m => added = m);

        // Act
        var result = await _service.SubmitAsync(ValidRequest(), Info);

        // Assert — mesaj ve onay tek SaveChanges'te
        result.IsSuccess.Should().BeTrue();
        added!.Status.Should().Be(ContactMessageStatus.Yeni);
        _consents.Verify(c => c.RecordAsync(It.Is<ConsentRecord>(r =>
            r.ContactMessage == added && r.ConsentType == ConsentType.Gizlilik && r.IpAddress == Info.IpAddress)), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData("evde", PackageLocation.Evde)]
    [InlineData("salonda", PackageLocation.Salonda)]
    public async Task SubmitAsync_ConvertsLocationToEnum(string location, PackageLocation expected)
    {
        // Arrange
        ContactMessage? added = null;
        _messages.Setup(r => r.AddAsync(It.IsAny<ContactMessage>())).Callback<ContactMessage>(m => added = m);

        // Act
        await _service.SubmitAsync(ValidRequest() with { Location = location }, Info);

        // Assert
        added!.Location.Should().Be(expected);
    }

    [Fact]
    public async Task SubmitAsync_WithMissingPackage_ReturnsValidationError()
    {
        // Act
        var result = await _service.SubmitAsync(ValidRequest() with { PackageId = 9 }, Info);

        // Assert
        result.Errors.Should().Contain("Seçilen paket bulunamadı.");
        _messages.Verify(r => r.AddAsync(It.IsAny<ContactMessage>()), Times.Never);
    }

    [Fact]
    public async Task SubmitAsync_WithUnknownLocation_ReturnsValidationError()
    {
        // Act
        var result = await _service.SubmitAsync(ValidRequest() with { Location = "ofiste" }, Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithUndefinedStatus_ReturnsValidationError()
    {
        // Act
        var result = await _service.UpdateStatusAsync(1, (ContactMessageStatus)99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Fact]
    public async Task UpdateStatusAsync_WithExistingId_ChangesStatus()
    {
        // Arrange
        var message = new ContactMessage { Id = 1, Status = ContactMessageStatus.Yeni };
        _messages.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(message);

        // Act
        var result = await _service.UpdateStatusAsync(1, ContactMessageStatus.Donuldu);

        // Assert
        result.IsSuccess.Should().BeTrue();
        message.Status.Should().Be(ContactMessageStatus.Donuldu);
    }

    [Fact]
    public async Task DeleteAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    private static CreateContactMessageRequest ValidRequest() => new()
    {
        Name = "Ali",
        Phone = "0555 111 22 33",
        Email = "ali@example.com",
        Message = "Merhaba",
        PrivacyAccepted = true
    };
}
