using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class LegalDocumentService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<PublishLegalDocumentRequest> validator,
    ILogger<LegalDocumentService> logger) : ILegalDocumentService
{
    private static readonly Dictionary<LegalDocumentType, string> DefaultTitles = new()
    {
        [LegalDocumentType.Kvkk] = "KVKK Aydınlatma Metni",
        [LegalDocumentType.AcikRiza] = "Açık Rıza Metni",
        [LegalDocumentType.Cerez] = "Çerez Politikası",
        [LegalDocumentType.IptalIade] = "İptal ve İade Koşulları",
        [LegalDocumentType.TicariIleti] = "Ticari Elektronik İleti Onay Metni",
        [LegalDocumentType.Gizlilik] = "Gizlilik Politikası",
        [LegalDocumentType.MesafeliSatis] = "Mesafeli Satış Sözleşmesi"
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

    public async Task<Result<LegalDocumentResponse>> PublishNewVersionAsync(LegalDocumentType type, PublishLegalDocumentRequest request)
    {
        if (!Enum.IsDefined(type))
            return Result<LegalDocumentResponse>.Invalid("Geçersiz belge türü.");

        var errors = await validator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result<LegalDocumentResponse>.Invalid(errors);

        var note = request.Note.Trim();
        var current = await unitOfWork.LegalDocuments.GetCurrentAsync(type);
        var now = DateTime.UtcNow;
        var document = new LegalDocument
        {
            Type = type,
            Title = current?.Title ?? DefaultTitles[type],
            Version = await unitOfWork.LegalDocuments.GetLatestVersionAsync(type) + 1,
            Note = note,
            ContentHtml = request.ContentHtml,
            PublishedAt = now,
            IsCurrent = true
        };

        // Revizyon geçmişi elle girilmez; her yeni sürümle birlikte otomatik yazılır
        document.Revisions.Add(new LegalDocumentRevision
        {
            Type = type, Version = document.Version, Note = note, RevisedAt = now
        });

        await unitOfWork.BeginTransactionAsync();
        try
        {
            // "Türe göre tek güncel sürüm" index'i nedeniyle önce eskisi kapatılır, sonra yenisi eklenir.
            if (current is not null)
            {
                current.IsCurrent = false;
                await unitOfWork.SaveChangesAsync();
            }

            await unitOfWork.LegalDocuments.AddAsync(document);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }

        logger.LogInformation("{Type} belgesinin {Version}. sürümü yayınlandı", type, document.Version);
        return Result<LegalDocumentResponse>.Success(mapper.Map<LegalDocumentResponse>(document));
    }
}
