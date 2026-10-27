using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface ISocialLinkService : ICrudService<SocialLinkResponse, CreateSocialLinkRequest, UpdateSocialLinkRequest>
{
    // Footer ve iletişim sayfasındaki linkler (WhatsApp butonları da buradan beslenir)
    Task<Result<IReadOnlyList<SocialLinkResponse>>> GetActiveAsync();
}
