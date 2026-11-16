using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Yasal metinler düzenlenmez: her değişiklik yeni bir PDF sürümüdür (eski onaylar eski sürüme bağlı kalır)
[Route("api/v1/admin/legal-documents")]
public class LegalDocumentsController(ILegalDocumentService legalDocumentService) : AdminControllerBase
{
    private const long MaxRequestBytes = 11 * 1024 * 1024; // PDF en fazla 10 MB + form alanları

    // Güncel sürüm → 200 / 404
    [HttpGet("{type}")]
    public async Task<ActionResult<LegalDocumentResponse>> GetCurrent(LegalDocumentType type) =>
        FromResult(await legalDocumentService.GetCurrentAsync(type));

    // Revizyon geçmişi (yeniden eskiye) → 200
    [HttpGet("{type}/revisions")]
    public async Task<ActionResult<IReadOnlyList<LegalDocumentRevisionResponse>>> GetRevisions(LegalDocumentType type) =>
        FromResult(await legalDocumentService.GetRevisionsAsync(type));

    // multipart/form-data: file (PDF) + note (değişiklik notu) → 200 + yeni sürüm / 400
    [HttpPost("{type}/versions")]
    [RequestSizeLimit(MaxRequestBytes)]
    public async Task<ActionResult<LegalDocumentResponse>> UploadNewVersion(LegalDocumentType type, IFormFile file, [FromForm] string note) =>
        FromResult(await legalDocumentService.UploadNewVersionAsync(type, file, note));
}
