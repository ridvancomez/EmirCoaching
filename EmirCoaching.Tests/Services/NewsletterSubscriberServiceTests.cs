using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class NewsletterSubscriberServiceTests
{
    private static readonly RequestInfo Info = new("10.0.0.1", "Test tarayıcı");

    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<INewsletterSubscriberRepository> _subscribers = new();
    private readonly Mock<IConsentRecordService> _consents = new();
    private readonly NewsletterSubscriberService _service;

    public NewsletterSubscriberServiceTests()
    {
        _uow.Setup(u => u.NewsletterSubscribers).Returns(_subscribers.Object);
        _service = new NewsletterSubscriberService(_uow.Object, TestMapper.Create(), _consents.Object,
            NullLogger<NewsletterSubscriberService>.Instance);
    }

    private static SubscribeNewsletterRequest Request(string email = "ali@example.com", bool privacy = true) => new()
    {
        Email = email,
        Consents = new() { [ConsentType.Gizlilik] = privacy, [ConsentType.TicariIleti] = true }
    };

    // ---- SubscribeAsync ----

    [Fact]
    public async Task SubscribeAsync_WithNewEmail_AddsSubscriberAndRecordsConsents()
    {
        // Arrange
        NewsletterSubscriber? added = null;
        _subscribers.Setup(r => r.AddAsync(It.IsAny<NewsletterSubscriber>())).Callback<NewsletterSubscriber>(s => added = s);

        // Act — e-posta kırpılıp küçük harfe çevrilerek kaydedilir
        var result = await _service.SubscribeAsync(Request("  Ali@Example.COM "), Info);

        // Assert
        result.IsSuccess.Should().BeTrue();
        added!.Email.Should().Be("ali@example.com");
        added.IsActive.Should().BeTrue();
        _consents.Verify(c => c.RecordAsync(It.Is<ConsentRecord>(r => r.NewsletterSubscriber == added && r.IpAddress == Info.IpAddress)), Times.Exactly(2));
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubscribeAsync_WhenIpLimitReached_ReturnsTooManyRequests()
    {
        // Arrange — bu IP'den son 24 saatte 3 yeni abonelik yapılmış
        _subscribers.Setup(r => r.CountByIpSinceAsync(Info.IpAddress, It.IsAny<DateTime>())).ReturnsAsync(3);

        // Act
        var result = await _service.SubscribeAsync(Request(), Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.TooManyRequests);
        _subscribers.Verify(r => r.AddAsync(It.IsAny<NewsletterSubscriber>()), Times.Never);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SubscribeAsync_ChecksIpLimitForLast24Hours()
    {
        // Act
        await _service.SubscribeAsync(Request(), Info);

        // Assert
        _subscribers.Verify(r => r.CountByIpSinceAsync(Info.IpAddress,
            It.Is<DateTime>(d => (DateTime.UtcNow - d - TimeSpan.FromHours(24)).Duration() < TimeSpan.FromMinutes(1))), Times.Once);
    }

    [Fact]
    public async Task SubscribeAsync_WithExistingEmail_ReactivatesWithoutCheckingIpLimit()
    {
        // Arrange — abonelikten çıkmış kişi tekrar abone oluyor
        var subscriber = new NewsletterSubscriber { Id = 4, Email = "ali@example.com", IsActive = false };
        _subscribers.Setup(r => r.GetByEmailAsync("ali@example.com")).ReturnsAsync(subscriber);

        // Act
        var result = await _service.SubscribeAsync(Request(), Info);

        // Assert
        result.IsSuccess.Should().BeTrue();
        subscriber.IsActive.Should().BeTrue();
        _subscribers.Verify(r => r.CountByIpSinceAsync(It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
        _subscribers.Verify(r => r.AddAsync(It.IsAny<NewsletterSubscriber>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData("gecersiz")]
    public async Task SubscribeAsync_WithInvalidEmail_ReturnsValidationError(string email)
    {
        // Act
        var result = await _service.SubscribeAsync(Request(email), Info);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SubscribeAsync_WithoutPrivacyConsent_ReturnsValidationError()
    {
        // Act
        var result = await _service.SubscribeAsync(Request(privacy: false), Info);

        // Assert
        result.Errors.Should().ContainSingle().Which.Should().Be("Gizlilik Politikası onayı zorunludur.");
    }

    [Fact]
    public async Task SubscribeAsync_WithUndefinedConsentType_ReturnsValidationError()
    {
        // Arrange — JSON'dan sayı olarak tanımsız bir onay türü gelmiş
        var request = Request() with { Consents = new() { [ConsentType.Gizlilik] = true, [(ConsentType)99] = true } };

        // Act
        var result = await _service.SubscribeAsync(request, Info);

        // Assert
        result.Errors.Should().ContainSingle().Which.Should().Be("Geçersiz onay türü.");
    }

    // ---- DeleteAsync (KVKK kalıcı silme) ----

    [Fact]
    public async Task DeleteAsync_WithExistingId_DeletesPermanentlyInTransaction()
    {
        // Arrange
        _subscribers.Setup(r => r.DeletePermanentlyAsync(4)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(4);

        // Assert — soft delete değil: Remove/SaveChanges yerine kalıcı silme çağrılır
        result.IsSuccess.Should().BeTrue();
        _uow.Verify(u => u.CommitAsync(), Times.Once);
        _subscribers.Verify(r => r.Remove(It.IsAny<NewsletterSubscriber>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }
}
