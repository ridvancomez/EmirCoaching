using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace EmirCoaching.Business.Services.Concrete;

public class BlogService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<AppUser> userManager,
    IValidator<CreateBlogPostDto> createValidator,
    IValidator<UpdateBlogPostDto> updateValidator) : IBlogService
{
    private const int RelatedPostCount = 3;
    private const string SlugInUse = "Bu slug başka bir yazıda kullanılıyor.";

    public async Task<PagedResult<BlogPostDto>> GetPagedAsync(string? categorySlug, int page, int size)
    {
        var posts = await unitOfWork.BlogPosts.GetPublishedPagedAsync(categorySlug, page, size);
        return new PagedResult<BlogPostDto>(
            mapper.Map<List<BlogPostDto>>(posts.Items), posts.TotalCount, posts.Page, posts.Size);
    }

    public async Task<BlogPostDetailDto?> GetDetailAsync(string slug)
    {
        var post = await unitOfWork.BlogPosts.GetBySlugAsync(slug);
        if (post is null)
            return null;

        var related = await unitOfWork.BlogPosts.GetRelatedAsync(post.Id, RelatedPostCount);
        return mapper.Map<BlogPostDetailDto>(post) with
        {
            RelatedPosts = mapper.Map<List<BlogPostDto>>(related)
        };
    }

    // Yeni yazı taslak olarak oluşur; yayına PublishAsync ile alınır.
    public async Task<Result<int>> CreateAsync(CreateBlogPostDto dto)
    {
        var errors = await createValidator.GetErrorsAsync(dto);
        errors.AddRange(await CheckReferencesAsync(dto));
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        if (await unitOfWork.BlogPosts.SlugExistsAsync(dto.Slug))
            return Result<int>.Conflict(SlugInUse);

        var post = mapper.Map<BlogPost>(dto);
        await unitOfWork.BlogPosts.AddAsync(post);
        await unitOfWork.SaveChangesAsync();
        return Result<int>.Success(post.Id);
    }

    public async Task<Result> UpdateAsync(UpdateBlogPostDto dto)
    {
        var errors = await updateValidator.GetErrorsAsync(dto);
        errors.AddRange(await CheckReferencesAsync(dto));
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var post = await unitOfWork.BlogPosts.GetByIdAsync(dto.Id);
        if (post is null)
            return Result.NotFound();

        if (await unitOfWork.BlogPosts.SlugExistsAsync(dto.Slug, dto.Id))
            return Result.Conflict(SlugInUse);

        mapper.Map(dto, post);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var post = await unitOfWork.BlogPosts.GetByIdAsync(id);
        if (post is null)
            return Result.NotFound();

        unitOfWork.BlogPosts.Remove(post);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> PublishAsync(int id)
    {
        var post = await unitOfWork.BlogPosts.GetByIdAsync(id);
        if (post is null)
            return Result.NotFound();

        post.IsPublished = true;
        post.PublishedAt ??= DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<List<string>> CheckReferencesAsync(CreateBlogPostDto dto)
    {
        var errors = new List<string>();

        if (await unitOfWork.Repository<BlogCategory>().GetByIdAsync(dto.CategoryId) is null)
            errors.Add("Kategori bulunamadı.");

        if (dto.RecommendedPackageId is { } packageId && await unitOfWork.Packages.GetByIdAsync(packageId) is null)
            errors.Add("Önerilen paket bulunamadı.");

        if (dto.AuthorId is { } authorId && await userManager.FindByIdAsync(authorId.ToString()) is null)
            errors.Add("Yazar bulunamadı.");

        return errors;
    }
}
