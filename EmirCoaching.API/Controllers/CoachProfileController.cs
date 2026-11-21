using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

// Tek kayıt: koç olarak işaretli kullanıcının bölüm bilgileri. Panelde profil ayarlarından (PUT /auth/me) düzenlenir.
[Route("api/v1/coach-profile")]
public class CoachProfileController(ICoachProfileService coachProfileService) : ApiControllerBase
{
    // Ana sayfa ve Hakkımda koç bölümü → 200 / 404
    [HttpGet]
    public async Task<ActionResult<CoachProfileResponse>> Get() => FromResult(await coachProfileService.GetAsync());
}
