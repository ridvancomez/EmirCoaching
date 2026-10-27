using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Blog kategorileri
[Route("api/v1/admin/blog-categories")]
public class BlogCategoriesController(IBlogCategoryService blogCategoryService) : AdminControllerBase
{
    // Pasifler dahil, sitedeki sırayla → 200
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        FromResult(await blogCategoryService.GetAllAsync());

    // → 200 / 404
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        FromResult(await blogCategoryService.GetByIdAsync(id));

    // → 201 / 400 / 409
    [HttpPost]
    public async Task<IActionResult> Create(CreateBlogCategoryRequest request) =>
        CreatedFromResult(await blogCategoryService.CreateAsync(request), nameof(GetById));

    // → 204 / 400 / 404 / 409
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateBlogCategoryRequest request) =>
        FromResult(await blogCategoryService.UpdateAsync(request with { Id = id }));

    // Soft delete → 204 / 404 / 409
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await blogCategoryService.DeleteAsync(id));
}
