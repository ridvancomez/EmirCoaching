using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

// Sitedeki koç bölümü; düzenleme profil ayarlarından (PUT /auth/me) yapılır
public interface ICoachProfileService
{
    Task<Result<CoachProfileResponse>> GetAsync();
}
