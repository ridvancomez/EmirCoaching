using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Diyet planı detay sayfasındaki "Nasıl Çalışır?" adımları
[Route("api/v1/admin/diet-program-steps")]
public class DietProgramStepsController(IDietProgramStepService dietProgramStepService) : AdminControllerBase
{
    // Sitedeki sırayla → 200
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DietProgramStepResponse>>> GetAll() =>
        FromResult(await dietProgramStepService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DietProgramStepResponse>> GetById(int id) =>
        FromResult(await dietProgramStepService.GetByIdAsync(id));

    // → 201 / 400
    [HttpPost]
    public async Task<ActionResult<CreatedResponse>> Create(CreateDietProgramStepRequest request) =>
        CreatedFromResult(await dietProgramStepService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDietProgramStepRequest request) =>
        FromResult(await dietProgramStepService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await dietProgramStepService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await dietProgramStepService.DeleteManyAsync(ids));
}
