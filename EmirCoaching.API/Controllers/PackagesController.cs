using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/packages")]
public class PackagesController(IPackageService packageService) : ApiControllerBase
{
    // Paketler sayfası (yalnızca aktifler)
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PackageResponse>>> GetList() => FromResult(await packageService.GetListAsync());

    // Paket detay sayfası: modüller, öne çıkanlar, yorumlar
    [HttpGet("{slug}")]
    public async Task<ActionResult<PackageDetailResponse>> GetDetail(string slug) => FromResult(await packageService.GetDetailAsync(slug));
}
