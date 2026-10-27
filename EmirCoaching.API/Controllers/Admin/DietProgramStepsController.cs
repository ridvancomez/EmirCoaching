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
    public async Task<IActionResult> GetAll() =>
        FromResult(await dietProgramStepService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await dietProgramStepService.GetByIdAsync(id));

    // → 201 / 400
    [HttpPost]
    public async Task<IActionResult> Create(CreateDietProgramStepRequest request) =>
        CreatedFromResult(await dietProgramStepService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateDietProgramStepRequest request) =>
        FromResult(await dietProgramStepService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await dietProgramStepService.DeleteAsync(id));
}
