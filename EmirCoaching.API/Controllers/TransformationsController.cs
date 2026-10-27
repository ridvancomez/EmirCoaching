using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/transformations")]
public class TransformationsController(ITransformationService transformationService) : ApiControllerBase
{
    // Değişimler sayfası: ?type=kilo-verme&sort=en-eskiler (varsayılan en yeniler)
    [HttpGet]
    public async Task<IActionResult> GetList(string? type, string? sort) =>
        FromResult(await transformationService.GetListAsync(type, sort));

    // Ana sayfadaki değişimler
    [HttpGet("home")]
    public async Task<IActionResult> GetHome() => FromResult(await transformationService.GetHomeAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetail(int id) => FromResult(await transformationService.GetDetailAsync(id));
}
