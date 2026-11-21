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

public class LegalDocumentServiceTests
{
    private const string Html = "<h2>1. Amaç</h2><p>Metin</p>";

    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ILegalDocumentRepository> _documents = new();
    private readonly LegalDocumentService _service;

    public LegalDocumentServiceTests()
    {
        _uow.Setup(u => u.LegalDocuments).Returns(_documents.Object);
        _service = new LegalDocumentService(_uow.Object, TestMapper.Create(), new PublishLegalDocumentRequestValidator(),
            NullLogger<LegalDocumentService>.Instance);
    }

    [Fact]
    public async Task PublishNewVersionAsync_ClosesCurrentAndAddsNextVersion()
    {
        // Arrange — 3. sürüm güncel
        var current = new LegalDocument { Id = 3, Type = LegalDocumentType.Kvkk, Title = "KVKK Metni", Version = 3, IsCurrent = true };
        _documents.Setup(r => r.GetCurrentAsync(LegalDocumentType.Kvkk)).ReturnsAsync(current);
        _documents.Setup(r => r.GetLatestVersionAsync(LegalDocumentType.Kvkk)).ReturnsAsync(3);
        LegalDocument? added = null;
        _documents.Setup(r => r.AddAsync(It.IsAny<LegalDocument>())).Callback<LegalDocument>(d => added = d);

        // Act
        var result = await _service.PublishNewVersionAsync(LegalDocumentType.Kvkk, Request("  Saklama süresi eklendi  "));

        // Assert — eski sürüm kapatılıp ayrı kaydedildi (tek güncel sürüm index'i), yenisi başlığı devraldı
        result.IsSuccess.Should().BeTrue();
        current.IsCurrent.Should().BeFalse();
        added!.Version.Should().Be(4);
        added.Title.Should().Be("KVKK Metni");
        added.ContentHtml.Should().Be(Html);
        added.FilePath.Should().BeNull();
        added.Note.Should().Be("Saklama süresi eklendi");
        added.Revisions.Should().ContainSingle(r => r.Version == 4 && r.Note == "Saklama süresi eklendi");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Exactly(2));
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Theory]
    [InlineData(LegalDocumentType.Cerez, "Çerez Politikası")]
    [InlineData(LegalDocumentType.MesafeliSatis, "Mesafeli Satış Sözleşmesi")]
    public async Task PublishNewVersionAsync_FirstVersion_UsesDefaultTitle(LegalDocumentType type, string expectedTitle)
    {
        // Arrange
        LegalDocument? added = null;
        _documents.Setup(r => r.AddAsync(It.IsAny<LegalDocument>())).Callback<LegalDocument>(d => added = d);

        // Act
        await _service.PublishNewVersionAsync(type, Request("İlk sürüm"));

        // Assert
        added!.Version.Should().Be(1);
        added.Title.Should().Be(expectedTitle);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData("<p>Metin</p>", "")]
    [InlineData("<p>Metin</p>", "   ")]
    [InlineData("", "Not")]
    public async Task PublishNewVersionAsync_WithoutContentOrNote_ReturnsValidationErrorAndDoesNotSave(string html, string note)
    {
        // Act
        var result = await _service.PublishNewVersionAsync(LegalDocumentType.Kvkk, new PublishLegalDocumentRequest { ContentHtml = html, Note = note });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task PublishNewVersionAsync_WithUndefinedType_ReturnsValidationError()
    {
        // Act
        var result = await _service.PublishNewVersionAsync((LegalDocumentType)99, Request("Not"));

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Fact]
    public async Task PublishNewVersionAsync_WhenSaveFails_RollsBackAndRethrows()
    {
        // Arrange
        _uow.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new InvalidOperationException());

        // Act
        var act = () => _service.PublishNewVersionAsync(LegalDocumentType.Kvkk, Request("Not"));

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _uow.Verify(u => u.RollbackAsync(), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Never);
    }

    private static PublishLegalDocumentRequest Request(string note) => new() { ContentHtml = Html, Note = note };
}
