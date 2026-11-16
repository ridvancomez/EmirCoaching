using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

// Tek kayıt: diyet planı detay sayfası (program pasifse 404)
[Route("api/v1/diet-program")]
public class DietProgramController(IDietProgramService dietProgramService) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<DietProgramResponse>> GetActive() => FromResult(await dietProgramService.GetActiveAsync());
}
