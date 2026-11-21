using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/certificates")]
public class CertificatesController(ICertificateService certificateService) : ApiControllerBase
{
    // Aktif sertifikalar, sıralı (ana sayfa koç bölümü ve Hakkımda) → 200
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CertificateResponse>>> GetActive() =>
        FromResult(await certificateService.GetActiveAsync());
}
