using EmirCoaching.API.Common;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/blog-posts")]
public class BlogPostsController(IBlogPostService blogPostService) : ApiControllerBase
{
    private const int DefaultPageSize = 9;

    // Yayınlanmış yazılar, isteğe bağlı kategori filtresiyle: ?category=beslenme&page=1&size=9
    [HttpGet]
    public async Task<ActionResult<PagedResult<BlogPostResponse>>> GetPaged(string? category, int page = 1, int size = DefaultPageSize) =>
        FromResult(await blogPostService.GetPagedAsync(category, page, size));

    // Blog sayfasının üstündeki öne çıkan yazı
    [HttpGet("featured")]
    public async Task<ActionResult<BlogPostResponse>> GetFeatured(string? category) =>
        FromResult(await blogPostService.GetFeaturedAsync(category));

    [HttpGet("{slug}")]
    public async Task<ActionResult<BlogPostDetailResponse>> GetDetail(string slug) => FromResult(await blogPostService.GetDetailAsync(slug));
}
