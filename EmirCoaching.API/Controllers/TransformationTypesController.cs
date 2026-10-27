using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/transformation-types")]
public class TransformationTypesController(ITransformationTypeService transformationTypeService) : ApiControllerBase
{
    // Değişimler sayfasındaki filtre sekmeleri
    [HttpGet]
    public async Task<IActionResult> GetAll() => FromResult(await transformationTypeService.GetAllAsync());
}
