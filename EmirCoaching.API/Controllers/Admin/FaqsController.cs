using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// SSS
[Route("api/v1/admin/faqs")]
public class FaqsController(IFaqService faqService) : AdminControllerBase
{
    // Pasifler dahil, sitedeki sırayla → 200
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FaqResponse>>> GetAll() =>
        FromResult(await faqService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<FaqResponse>> GetById(int id) =>
        FromResult(await faqService.GetByIdAsync(id));

    // → 201 / 400 / 409
    [HttpPost]
    public async Task<ActionResult<CreatedResponse>> Create(CreateFaqRequest request) =>
        CreatedFromResult(await faqService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateFaqRequest request) =>
        FromResult(await faqService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await faqService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await faqService.DeleteManyAsync(ids));
}
