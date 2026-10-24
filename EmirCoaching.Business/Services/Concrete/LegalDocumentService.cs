using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace EmirCoaching.Business.Services.Concrete;

public class LegalDocumentService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IFileStorageService fileStorage) : ILegalDocumentService
{
    private const string UploadFolder = "yasal-metinler";

    private static readonly Dictionary<LegalDocumentType, string> DefaultTitles = new()
    {
        [LegalDocumentType.Kvkk] = "KVKK Aydınlatma Metni",
        [LegalDocumentType.AcikRiza] = "Açık Rıza Metni",
        [LegalDocumentType.Cerez] = "Çerez Politikası",
        [LegalDocumentType.IptalIade] = "İptal ve İade Koşulları",
        [LegalDocumentType.TicariIleti] = "Ticari Elektronik İleti Onay Metni",
        [LegalDocumentType.Gizlilik] = "Gizlilik Politikası"
    };

    public async Task<LegalDocumentDto?> GetCurrentAsync(LegalDocumentType type)
    {
        var document = await unitOfWork.LegalDocuments.GetCurrentAsync(type);
        return document is null ? null : mapper.Map<LegalDocumentDto>(document);
    }

    public async Task<Result<LegalDocumentDto>> UploadNewVersionAsync(LegalDocumentType type, IFormFile file)
    {
        if (!Enum.IsDefined(type))
            return Result<LegalDocumentDto>.Invalid("Geçersiz belge türü.");

        string filePath;
        try
        {
            filePath = await fileStorage.SavePdfAsync(file, UploadFolder);
        }
        catch (ArgumentException ex)
        {
            return Result<LegalDocumentDto>.Invalid(ex.Message);
        }

        try
        {
            var current = await unitOfWork.LegalDocuments.GetCurrentAsync(type);
            var document = new LegalDocument
            {
                Type = type,
                Title = current?.Title ?? DefaultTitles[type],
                Version = await unitOfWork.LegalDocuments.GetLatestVersionAsync(type) + 1,
                FilePath = filePath,
                PublishedAt = DateTime.UtcNow,
                IsCurrent = true
            };

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

            return Result<LegalDocumentDto>.Success(mapper.Map<LegalDocumentDto>(document));
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            await fileStorage.DeleteAsync(filePath);
            throw;
        }
    }
}
