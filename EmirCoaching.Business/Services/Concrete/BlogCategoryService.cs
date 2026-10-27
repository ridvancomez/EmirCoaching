using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class BlogCategoryService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateBlogCategoryRequest> createValidator,
    IValidator<UpdateBlogCategoryRequest> updateValidator,
    ILogger<BlogCategoryService> logger)
    : CrudService<BlogCategory, BlogCategoryResponse, CreateBlogCategoryRequest, UpdateBlogCategoryRequest>(
        unitOfWork.BlogCategories, unitOfWork, mapper, createValidator, updateValidator, logger), IBlogCategoryService
{
    protected override async Task<Result> CheckAsync(CreateBlogCategoryRequest request, int? id) =>
        await UnitOfWork.BlogCategories.SlugExistsAsync(request.Slug, id)
            ? Result.Conflict("Bu slug başka bir kategoride kullanılıyor.")
            : Result.Success();

    protected override async Task<Result> CheckDeleteAsync(BlogCategory category) =>
        await UnitOfWork.BlogPosts.AnyInCategoryAsync(category.Id)
            ? Result.Conflict("Bu kategoride yazılar var; önce yazıları başka kategoriye taşıyın.")
            : Result.Success();
}
