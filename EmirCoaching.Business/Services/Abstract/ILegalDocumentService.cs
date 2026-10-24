using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Http;

namespace EmirCoaching.Business.Services.Abstract;

public interface ILegalDocumentService
{
    Task<LegalDocumentDto?> GetCurrentAsync(LegalDocumentType type);

    // Admin: yeni PDF yeni sürüm olur, öncekinin IsCurrent'i kapanır (eski onaylar eski sürüme bağlı kalır)
    Task<Result<LegalDocumentDto>> UploadNewVersionAsync(LegalDocumentType type, IFormFile file);
}
