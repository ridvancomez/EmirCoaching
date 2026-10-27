using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

// Tek kayıt: iletişim bilgileri ve çalışma saatleri (footer, iletişim sayfası)
[Route("api/v1/site-settings")]
public class SiteSettingsController(ISiteSettingService siteSettingService) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get() => FromResult(await siteSettingService.GetAsync());
}
