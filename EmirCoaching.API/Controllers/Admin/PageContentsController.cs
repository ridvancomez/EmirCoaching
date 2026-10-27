using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Sayfalar seed ile gelir ve sabittir: yalnızca içerik güncellenir
[Route("api/v1/admin/page-contents")]
public class PageContentsController(IPageContentService pageContentService) : AdminControllerBase
{
    // Tüm sayfalar → 200
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        FromResult(await pageContentService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{pageKey}")]
    public async Task<IActionResult> GetByPageKey(string pageKey) =>
        FromResult(await pageContentService.GetByPageKeyAsync(pageKey));

    // → 204 / 400 / 404
    [HttpPut("{pageKey}")]
    public async Task<IActionResult> Update(string pageKey, UpdatePageContentRequest request) =>
        FromResult(await pageContentService.UpdateAsync(pageKey, request));
}
