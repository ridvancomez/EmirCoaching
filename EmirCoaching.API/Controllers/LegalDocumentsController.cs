using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/legal-documents")]
public class LegalDocumentsController(ILegalDocumentService legalDocumentService) : ApiControllerBase
{
    // Türün güncel sürümü: /legal-documents/Gizlilik
    [HttpGet("{type}")]
    public async Task<IActionResult> GetCurrent(LegalDocumentType type) =>
        FromResult(await legalDocumentService.GetCurrentAsync(type));

    // Gizlilik sayfasındaki "Revizyon Geçmişi"
    [HttpGet("{type}/revisions")]
    public async Task<IActionResult> GetRevisions(LegalDocumentType type) =>
        FromResult(await legalDocumentService.GetRevisionsAsync(type));
}
