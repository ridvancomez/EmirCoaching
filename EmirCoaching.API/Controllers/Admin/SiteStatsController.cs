using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Sayaçlar
[Route("api/v1/admin/site-stats")]
public class SiteStatsController(ISiteStatService siteStatService) : AdminControllerBase
{
    // Pasifler dahil, sitedeki sırayla → 200
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        FromResult(await siteStatService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await siteStatService.GetByIdAsync(id));

    // → 201 / 400 / 409
    [HttpPost]
    public async Task<IActionResult> Create(CreateSiteStatRequest request) =>
        CreatedFromResult(await siteStatService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateSiteStatRequest request) =>
        FromResult(await siteStatService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await siteStatService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await siteStatService.DeleteManyAsync(ids));
}
