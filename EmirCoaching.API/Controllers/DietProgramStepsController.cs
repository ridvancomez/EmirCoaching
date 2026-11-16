using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/diet-program-steps")]
public class DietProgramStepsController(IDietProgramStepService dietProgramStepService) : ApiControllerBase
{
    // Diyet planı detay sayfasındaki "Nasıl Çalışır?" adımları
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DietProgramStepResponse>>> GetAll() => FromResult(await dietProgramStepService.GetAllAsync());
}
