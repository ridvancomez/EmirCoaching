using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/packages")]
public class PackagesController(IPackageService packageService) : ApiControllerBase
{
    // Paketler sayfası (yalnızca aktifler)
    [HttpGet]
    public async Task<IActionResult> GetList() => FromResult(await packageService.GetListAsync());

    // Paket detay sayfası: modüller, öne çıkanlar, yorumlar
    [HttpGet("{slug}")]
    public async Task<IActionResult> GetDetail(string slug) => FromResult(await packageService.GetDetailAsync(slug));
}
