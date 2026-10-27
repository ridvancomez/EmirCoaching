using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Tek kayıt: oluşturma ve silme yok
[Route("api/v1/admin/site-settings")]
public class SiteSettingsController(ISiteSettingService siteSettingService) : AdminControllerBase
{
    // → 200
    [HttpGet]
    public async Task<IActionResult> Get() =>
        FromResult(await siteSettingService.GetAsync());

    // → 204 / 400
    [HttpPut]
    public async Task<IActionResult> Update(UpdateSiteSettingRequest request) =>
        FromResult(await siteSettingService.UpdateAsync(request));
}
