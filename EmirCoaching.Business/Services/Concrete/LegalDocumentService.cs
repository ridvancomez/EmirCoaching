using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class LegalDocumentService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IFileStorageService fileStorage,
    ILogger<LegalDocumentService> logger) : ILegalDocumentService
{
    private const string UploadFolder = "yasal-metinler";
    private const int NoteMaxLength = 300;

    private static readonly Dictionary<LegalDocumentType, string> DefaultTitles = new()
    {
        [LegalDocumentType.Kvkk] = "KVKK Aydınlatma Metni",
        [LegalDocumentType.AcikRiza] = "Açık Rıza Metni",
        [LegalDocumentType.Cerez] = "Çerez Politikası",
        [LegalDocumentType.IptalIade] = "İptal ve İade Koşulları",
        [LegalDocumentType.TicariIleti] = "Ticari Elektronik İleti Onay Metni",
        [LegalDocumentType.Gizlilik] = "Gizlilik Politikası"
    };

    public async Task<Result<LegalDocumentResponse>> GetCurrentAsync(LegalDocumentType type)
    {
        var document = await unitOfWork.LegalDocuments.GetCurrentAsync(type);
        return document is null
            ? Result<LegalDocumentResponse>.NotFound("Belge bulunamadı.")
            : Result<LegalDocumentResponse>.Success(mapper.Map<LegalDocumentResponse>(document));
    }

    public async Task<Result<IReadOnlyList<LegalDocumentRevisionResponse>>> GetRevisionsAsync(LegalDocumentType type) =>
        Result<IReadOnlyList<LegalDocumentRevisionResponse>>.Success(
            mapper.Map<List<LegalDocumentRevisionResponse>>(await unitOfWork.LegalDocuments.GetRevisionsAsync(type)));

    public async Task<Result<LegalDocumentResponse>> UploadNewVersionAsync(LegalDocumentType type, IFormFile file, string note)
    {
        if (!Enum.IsDefined(type))
            return Result<LegalDocumentResponse>.Invalid("Geçersiz belge türü.");

        note = note.Trim();
        if (note.Length is 0 or > NoteMaxLength)
            return Result<LegalDocumentResponse>.Invalid($"Değişiklik notu zorunludur (en fazla {NoteMaxLength} karakter).");

        string filePath;
        try
        {
            filePath = await fileStorage.SavePdfAsync(file, UploadFolder);
        }
        catch (ArgumentException ex)
        {
            return Result<LegalDocumentResponse>.Invalid(ex.Message);
        }

        try
        {
            var current = await unitOfWork.LegalDocuments.GetCurrentAsync(type);
            var now = DateTime.UtcNow;
            var document = new LegalDocument
            {
                Type = type,
                Title = current?.Title ?? DefaultTitles[type],
                Version = await unitOfWork.LegalDocuments.GetLatestVersionAsync(type) + 1,
                Note = note,
                FilePath = filePath,
                PublishedAt = now,
                IsCurrent = true
            };

            // Revizyon geçmişi elle girilmez; her yeni sürümle birlikte otomatik yazılır
            document.Revisions.Add(new LegalDocumentRevision
            {
                Type = type, Version = document.Version, Note = note, RevisedAt = now
            });

            // "Türe göre tek güncel sürüm" index'i nedeniyle önce eskisi kapatılır, sonra yenisi eklenir.
            await unitOfWork.BeginTransactionAsync();
            if (current is not null)
            {
                current.IsCurrent = false;
                await unitOfWork.SaveChangesAsync();
            }

            await unitOfWork.LegalDocuments.AddAsync(document);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();

            logger.LogInformation("{Type} belgesinin {Version}. sürümü yayınlandı", type, document.Version);

            return Result<LegalDocumentResponse>.Success(mapper.Map<LegalDocumentResponse>(document));
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            await fileStorage.DeleteAsync(filePath);
            throw;
        }
    }
}
