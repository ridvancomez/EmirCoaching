using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class LegalDocumentServiceTests
{
    private const string SavedPath = "/uploads/yasal-metinler/yeni.pdf";

    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ILegalDocumentRepository> _documents = new();
    private readonly Mock<IFileStorageService> _storage = new();
    private readonly IFormFile _file = Mock.Of<IFormFile>();
    private readonly LegalDocumentService _service;

    public LegalDocumentServiceTests()
    {
        _uow.Setup(u => u.LegalDocuments).Returns(_documents.Object);
        _storage.Setup(s => s.SavePdfAsync(_file, "yasal-metinler")).ReturnsAsync(SavedPath);

        _service = new LegalDocumentService(_uow.Object, TestMapper.Create(), _storage.Object,
            NullLogger<LegalDocumentService>.Instance);
    }

    [Fact]
    public async Task UploadNewVersionAsync_ClosesCurrentAndAddsNextVersion()
    {
        // Arrange — 3. sürüm güncel
        var current = new LegalDocument { Id = 3, Type = LegalDocumentType.Kvkk, Title = "KVKK Metni", Version = 3, IsCurrent = true };
        _documents.Setup(r => r.GetCurrentAsync(LegalDocumentType.Kvkk)).ReturnsAsync(current);
        _documents.Setup(r => r.GetLatestVersionAsync(LegalDocumentType.Kvkk)).ReturnsAsync(3);
        LegalDocument? added = null;
        _documents.Setup(r => r.AddAsync(It.IsAny<LegalDocument>())).Callback<LegalDocument>(d => added = d);

        // Act
        var result = await _service.UploadNewVersionAsync(LegalDocumentType.Kvkk, _file, "  Saklama süresi eklendi  ");

        // Assert — eski sürüm kapatılıp ayrı kaydedildi (tek güncel sürüm index'i), yenisi başlığı devraldı
        result.IsSuccess.Should().BeTrue();
        current.IsCurrent.Should().BeFalse();
        added!.Version.Should().Be(4);
        added.Title.Should().Be("KVKK Metni");
        added.FilePath.Should().Be(SavedPath);
        added.Note.Should().Be("Saklama süresi eklendi");
        added.Revisions.Should().ContainSingle(r => r.Version == 4 && r.Note == "Saklama süresi eklendi");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Exactly(2));
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task UploadNewVersionAsync_FirstVersion_UsesDefaultTitle()
    {
        // Arrange
        LegalDocument? added = null;
        _documents.Setup(r => r.AddAsync(It.IsAny<LegalDocument>())).Callback<LegalDocument>(d => added = d);

        // Act
        await _service.UploadNewVersionAsync(LegalDocumentType.Cerez, _file, "İlk sürüm");

        // Assert
        added!.Version.Should().Be(1);
        added.Title.Should().Be("Çerez Politikası");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task UploadNewVersionAsync_WithoutNote_ReturnsValidationErrorAndDoesNotSaveFile(string note)
    {
        // Act
        var result = await _service.UploadNewVersionAsync(LegalDocumentType.Kvkk, _file, note);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _storage.Verify(s => s.SavePdfAsync(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task UploadNewVersionAsync_WithUndefinedType_ReturnsValidationError()
    {
        // Act
        var result = await _service.UploadNewVersionAsync((LegalDocumentType)99, _file, "Not");

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Fact]
    public async Task UploadNewVersionAsync_WithInvalidFile_ReturnsStorageMessageAsValidationError()
    {
        // Arrange — dosya kuralları (boyut, tür, imza) FileStorageService'te; mesaj kullanıcıya iletilir
        _storage.Setup(s => s.SavePdfAsync(_file, It.IsAny<string>())).ThrowsAsync(new ArgumentException("Dosya içeriği uzantısıyla uyuşmuyor."));

        // Act
        var result = await _service.UploadNewVersionAsync(LegalDocumentType.Kvkk, _file, "Not");

        // Assert
        result.Errors.Should().ContainSingle().Which.Should().Be("Dosya içeriği uzantısıyla uyuşmuyor.");
        _documents.Verify(r => r.AddAsync(It.IsAny<LegalDocument>()), Times.Never);
    }

    [Fact]
    public async Task UploadNewVersionAsync_WhenSaveFails_RollsBackAndDeletesUploadedFile()
    {
        // Arrange
        _uow.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new InvalidOperationException());

        // Act
        var act = () => _service.UploadNewVersionAsync(LegalDocumentType.Kvkk, _file, "Not");

        // Assert — kayıt oluşmadıysa diske yazılan PDF sahipsiz kalmasın
        await act.Should().ThrowAsync<InvalidOperationException>();
        _uow.Verify(u => u.RollbackAsync(), Times.Once);
        _storage.Verify(s => s.DeleteAsync(SavedPath), Times.Once);
    }
}
