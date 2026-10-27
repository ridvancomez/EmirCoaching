using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/social-links")]
public class SocialLinksController(ISocialLinkService socialLinkService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetActive() => FromResult(await socialLinkService.GetActiveAsync());
}
