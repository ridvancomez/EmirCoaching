using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/page-contents")]
public class PageContentsController(IPageContentService pageContentService) : ApiControllerBase
{
    // Sayfa başlık alanı (hero) içeriği
    [HttpGet("{pageKey}")]
    public async Task<ActionResult<PageContentResponse>> GetByPageKey(string pageKey) =>
        FromResult(await pageContentService.GetByPageKeyAsync(pageKey));
}
