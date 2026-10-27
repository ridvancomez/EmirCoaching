using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Http;

namespace EmirCoaching.Business.Services.Abstract;

public interface ILegalDocumentService
{
    Task<Result<LegalDocumentResponse>> GetCurrentAsync(LegalDocumentType type);

    // Yeniden eskiye revizyon geçmişi
    Task<Result<IReadOnlyList<LegalDocumentRevisionResponse>>> GetRevisionsAsync(LegalDocumentType type);

    // Admin: yeni PDF yeni sürüm olur, öncekinin IsCurrent'i kapanır (eski onaylar eski sürüme bağlı kalır).
    // Sürüm numarası ve tarih otomatik atanır; not ile birlikte revizyon geçmişine de yazılır.
    Task<Result<LegalDocumentResponse>> UploadNewVersionAsync(LegalDocumentType type, IFormFile file, string note);
}
