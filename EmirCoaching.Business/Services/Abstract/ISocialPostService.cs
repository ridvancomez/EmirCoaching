using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

public interface ISocialPostService : ICrudService<SocialPostResponse, CreateSocialPostRequest, UpdateSocialPostRequest>
{
    // Sitedeki sosyal medya bölümü (Instagram son 3 post, YouTube videoları)
    Task<Result<IReadOnlyList<SocialPostResponse>>> GetActiveByPlatformAsync(SocialPlatform platform, int count);
}
