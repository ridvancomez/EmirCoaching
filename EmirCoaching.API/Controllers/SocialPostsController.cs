using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/social-posts")]
public class SocialPostsController(ISocialPostService socialPostService) : ApiControllerBase
{
    private const int DefaultCount = 3;

    // Ana sayfadaki son postlar: ?platform=Instagram&count=3 (servis en fazla 12 döner)
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<SocialPostResponse>>> GetActiveByPlatform(SocialPlatform platform, int count = DefaultCount) =>
        FromResult(await socialPostService.GetActiveByPlatformAsync(platform, count));
}
