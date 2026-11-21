using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Yasal metinler düzenlenmez: her değişiklik yeni bir sürümdür (eski onaylar eski sürüme bağlı kalır)
[Route("api/v1/admin/legal-documents")]
public class LegalDocumentsController(ILegalDocumentService legalDocumentService) : AdminControllerBase
{
    // Güncel sürüm → 200 / 404
    [HttpGet("{type}")]
    public async Task<ActionResult<LegalDocumentResponse>> GetCurrent(LegalDocumentType type) =>
        FromResult(await legalDocumentService.GetCurrentAsync(type));

    // Revizyon geçmişi (yeniden eskiye) → 200
    [HttpGet("{type}/revisions")]
    public async Task<ActionResult<IReadOnlyList<LegalDocumentRevisionResponse>>> GetRevisions(LegalDocumentType type) =>
        FromResult(await legalDocumentService.GetRevisionsAsync(type));

    // { contentHtml, note } → 200 + yeni sürüm / 400
    [HttpPost("{type}/versions")]
    public async Task<ActionResult<LegalDocumentResponse>> PublishNewVersion(LegalDocumentType type, PublishLegalDocumentRequest request) =>
        FromResult(await legalDocumentService.PublishNewVersionAsync(type, request));
}
