using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Koçun sertifikaları
[Route("api/v1/admin/certificates")]
public class CertificatesController(ICertificateService certificateService) : AdminControllerBase
{
    // Pasifler dahil, sitedeki sırayla → 200
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CertificateResponse>>> GetAll() =>
        FromResult(await certificateService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CertificateResponse>> GetById(int id) =>
        FromResult(await certificateService.GetByIdAsync(id));

    // → 201 / 400
    [HttpPost]
    public async Task<ActionResult<CreatedResponse>> Create(CreateCertificateRequest request) =>
        CreatedFromResult(await certificateService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateCertificateRequest request) =>
        FromResult(await certificateService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await certificateService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri; gövde: [3, 7, 12] → 204 / 400 / 404
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await certificateService.DeleteManyAsync(ids));
}
