using EmirCoaching.API.Common;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

[Route("api/v1/admin/blog-posts")]
public class BlogPostsController(IBlogPostService blogPostService) : AdminControllerBase
{
    private const int DefaultPageSize = 20;

    // Taslaklar dahil, yeniden eskiye: ?categoryId=2&isPublished=false&page=1&size=20 → 200
    [HttpGet]
    public async Task<ActionResult<PagedResult<BlogPostResponse>>> GetAll(int? categoryId, bool? isPublished, int page = 1, int size = DefaultPageSize) =>
        FromResult(await blogPostService.GetAllPagedAsync(categoryId, isPublished, page, size));

    // Düzenleme formu (taslak olsa da) → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BlogPostDetailResponse>> GetById(int id) =>
        FromResult(await blogPostService.GetByIdAsync(id));

    // Taslak olarak kaydedilir; yazar seçilmezse giriş yapan kullanıcı → 201 / 400 / 409
    [HttpPost]
    public async Task<ActionResult<CreatedResponse>> Create(CreateBlogPostRequest request) =>
        CreatedFromResult(await blogPostService.CreateAsync(request with { AuthorId = request.AuthorId ?? CurrentUserId }), nameof(GetById));

    // → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateBlogPostRequest request) =>
        FromResult(await blogPostService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await blogPostService.DeleteAsync(id));

    // Ya hepsi ya hiçbiri: biri bulunamaz veya silinemezse hiçbiri silinmez; gövde: [3, 7, 12] → 204 / 400 / 404 / 409
    [HttpPost("bulk-delete")]
    public async Task<IActionResult> DeleteMany(List<int> ids) =>
        FromResult(await blogPostService.DeleteManyAsync(ids));

    // Taslağı yayınlar → 204 / 404
    [HttpPost("{id:int}/publish")]
    public async Task<IActionResult> Publish(int id) =>
        FromResult(await blogPostService.PublishAsync(id));

    // Taslağa döndürür, sitede görünmez → 204 / 404
    [HttpPost("{id:int}/unpublish")]
    public async Task<IActionResult> Unpublish(int id) =>
        FromResult(await blogPostService.UnpublishAsync(id));
}
