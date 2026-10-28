using AwesomeAssertions;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Moq;

namespace EmirCoaching.Tests.Services;

public class ConsentRecordServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IConsentRecordRepository> _consents = new();
    private readonly Mock<ILegalDocumentRepository> _documents = new();
    private readonly ConsentRecordService _service;

    public ConsentRecordServiceTests()
    {
        _uow.Setup(u => u.ConsentRecords).Returns(_consents.Object);
        _uow.Setup(u => u.LegalDocuments).Returns(_documents.Object);
        _service = new ConsentRecordService(_uow.Object);
    }

    [Theory]
    [InlineData(ConsentType.Gizlilik, LegalDocumentType.Gizlilik)]
    [InlineData(ConsentType.AcikRizaSaglik, LegalDocumentType.AcikRiza)]
    [InlineData(ConsentType.TicariIleti, LegalDocumentType.TicariIleti)]
    public async Task RecordAsync_LinksCurrentVersionOfMatchingLegalDocument(ConsentType consent, LegalDocumentType document)
    {
        // Arrange — onay, onaylandığı andaki metin sürümüne bağlanır
        _documents.Setup(r => r.GetCurrentAsync(document)).ReturnsAsync(new LegalDocument { Id = 42 });
        var record = Record(consent);

        // Act
        await _service.RecordAsync(record);

        // Assert
        record.LegalDocumentId.Should().Be(42);
        _consents.Verify(r => r.AddAsync(record), Times.Once);
    }

    [Fact]
    public async Task RecordAsync_SetsGrantedAtAndTruncatesLongValues()
    {
        // Arrange
        var record = Record(ConsentType.Gizlilik);
        record.IpAddress = new string('1', 60);
        record.UserAgent = new string('a', 1000);

        // Act
        await _service.RecordAsync(record);

        // Assert — kolon uzunlukları: IP 45 (IPv6), tarayıcı 512
        record.GrantedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        record.IpAddress.Should().HaveLength(45);
        record.UserAgent.Should().HaveLength(512);
    }

    [Fact]
    public async Task RecordAsync_WithoutOwner_Throws()
    {
        // Arrange — onay bir forma bağlı olmalı; programlama hatasıdır, 500'e gider
        var record = new ConsentRecord { ConsentType = ConsentType.Gizlilik };

        // Act
        var act = () => _service.RecordAsync(record);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _consents.Verify(r => r.AddAsync(It.IsAny<ConsentRecord>()), Times.Never);
    }

    [Fact]
    public async Task RecordAsync_WithTwoOwners_Throws()
    {
        // Arrange
        var record = Record(ConsentType.Gizlilik);
        record.ContactMessage = new ContactMessage();

        // Act
        var act = () => _service.RecordAsync(record);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    private static ConsentRecord Record(ConsentType type) => new()
    {
        ConsentType = type,
        IsGranted = true,
        IpAddress = "10.0.0.1",
        UserAgent = "Test tarayıcı",
        NewsletterSubscriber = new NewsletterSubscriber()
    };
}
