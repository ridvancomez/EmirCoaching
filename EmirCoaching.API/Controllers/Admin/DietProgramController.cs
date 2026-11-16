using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Tek kayıt: oluşturma ve silme yok
[Route("api/v1/admin/diet-program")]
public class DietProgramController(IDietProgramService dietProgramService) : AdminControllerBase
{
    // Pasif olsa da döner → 200
    [HttpGet]
    public async Task<ActionResult<DietProgramResponse>> Get() =>
        FromResult(await dietProgramService.GetAsync());

    // "Nasıl Çalışır?" adımları ayrı: admin/diet-program-steps → 204 / 400
    [HttpPut]
    public async Task<IActionResult> Update(UpdateDietProgramRequest request) =>
        FromResult(await dietProgramService.UpdateAsync(request));
}
