using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Ana sayfa "Satın aldıktan sonra ne oluyor?" adımları
[Route("api/v1/admin/purchase-steps")]
public class PurchaseStepsController(IPurchaseStepService purchaseStepService) : AdminControllerBase
{
    // Sıralı → 200
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PurchaseStepResponse>>> GetAll() =>
        FromResult(await purchaseStepService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PurchaseStepResponse>> GetById(int id) =>
        FromResult(await purchaseStepService.GetByIdAsync(id));

    // → 201 / 400
    [HttpPost]
    public async Task<ActionResult<CreatedResponse>> Create(CreatePurchaseStepRequest request) =>
        CreatedFromResult(await purchaseStepService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdatePurchaseStepRequest request) =>
        FromResult(await purchaseStepService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await purchaseStepService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri; gövde: [3, 7, 12] → 204 / 400 / 404
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await purchaseStepService.DeleteManyAsync(ids));
}
