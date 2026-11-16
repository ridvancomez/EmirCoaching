using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/blog-categories")]
public class BlogCategoriesController(IBlogCategoryService blogCategoryService) : ApiControllerBase
{
    // Blog sayfasındaki filtre sekmeleri
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BlogCategoryResponse>>> GetAll() => FromResult(await blogCategoryService.GetAllAsync());
}
