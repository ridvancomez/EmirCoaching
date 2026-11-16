using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/site-stats")]
public class SiteStatsController(ISiteStatService siteStatService) : ApiControllerBase
{
    // Ana sayfadaki sayaçlar
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SiteStatResponse>>> GetAll() => FromResult(await siteStatService.GetAllAsync());
}
