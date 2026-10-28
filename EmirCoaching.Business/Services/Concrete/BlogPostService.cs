using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Mapping;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class BlogPostService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    UserManager<AppUser> userManager,
    IValidator<CreateBlogPostRequest> createValidator,
    IValidator<UpdateBlogPostRequest> updateValidator,
    IUploadCleanupService uploadCleanup,
    ILogger<BlogPostService> logger) : IBlogPostService
{
    private const int RelatedPostCount = 3;
    private const string SlugInUse = "Bu slug başka bir yazıda kullanılıyor.";
    private const string PostNotFound = "Yazı bulunamadı.";

    public async Task<Result<PagedResult<BlogPostResponse>>> GetPagedAsync(string? categorySlug, int page, int size)
    {
        var posts = await unitOfWork.BlogPosts.GetPublishedPagedAsync(categorySlug, page, size);
        return Result<PagedResult<BlogPostResponse>>.Success(mapper.MapPaged<BlogPost, BlogPostResponse>(posts));
    }

    public async Task<Result<PagedResult<BlogPostResponse>>> GetAllPagedAsync(int? categoryId, bool? isPublished, int page, int size)
    {
        var posts = await unitOfWork.BlogPosts.GetPagedAsync(categoryId, isPublished, page, size);
        return Result<PagedResult<BlogPostResponse>>.Success(mapper.MapPaged<BlogPost, BlogPostResponse>(posts));
    }

    public async Task<Result<BlogPostDetailResponse>> GetByIdAsync(int id)
    {
        var post = await unitOfWork.BlogPosts.GetDetailByIdAsync(id);
        if (post is null)
        {
            logger.LogWarning("Blog yazısı {Id} bulunamadı", id);
            return Result<BlogPostDetailResponse>.NotFound(PostNotFound);
        }

        return Result<BlogPostDetailResponse>.Success(mapper.Map<BlogPostDetailResponse>(post));
    }

    public async Task<Result<BlogPostDetailResponse>> GetDetailAsync(string slug)
    {
        var post = await unitOfWork.BlogPosts.GetBySlugAsync(slug);
        if (post is null)
        {
            logger.LogWarning("Blog yazısı {Slug} bulunamadı", slug);
            return Result<BlogPostDetailResponse>.NotFound(PostNotFound);
        }

        var related = await unitOfWork.BlogPosts.GetRelatedAsync(post.Id, RelatedPostCount);
        return Result<BlogPostDetailResponse>.Success(mapper.Map<BlogPostDetailResponse>(post) with
        {
            RelatedPosts = mapper.Map<List<BlogPostResponse>>(related)
        });
    }

    public async Task<Result<BlogPostResponse>> GetFeaturedAsync(string? categorySlug)
    {
        var post = await unitOfWork.BlogPosts.GetFeaturedForDisplayAsync(categorySlug);
        return post is null
            ? Result<BlogPostResponse>.NotFound("Yayında yazı yok.")
            : Result<BlogPostResponse>.Success(mapper.Map<BlogPostResponse>(post));
    }

    // Yeni yazı taslak olarak oluşur; yayına PublishAsync ile alınır.
    public async Task<Result<int>> CreateAsync(CreateBlogPostRequest request)
    {
        var errors = await createValidator.GetErrorsAsync(request);
        errors.AddRange(await CheckReferencesAsync(request));
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        if (await unitOfWork.BlogPosts.SlugExistsAsync(request.Slug))
            return Result<int>.Conflict(SlugInUse);

        var post = mapper.Map<BlogPost>(request);
        await SetTagsAsync(post, request.Tags);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            await ReleaseFeaturedAsync(request.CategoryId, request.IsFeatured, currentId: null);
            await unitOfWork.BlogPosts.AddAsync(post);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }

        logger.LogInformation("Blog yazısı {Id} oluşturuldu", post.Id);
        return Result<int>.Success(post.Id);
    }

    public async Task<Result> UpdateAsync(UpdateBlogPostRequest request)
    {
        var errors = await updateValidator.GetErrorsAsync(request);
        errors.AddRange(await CheckReferencesAsync(request));
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var post = await unitOfWork.BlogPosts.GetByIdWithTagsAsync(request.Id);
        if (post is null)
            return Result.NotFound(PostNotFound);

        if (await unitOfWork.BlogPosts.SlugExistsAsync(request.Slug, request.Id))
            return Result.Conflict(SlugInUse);

        List<string> oldUploads;
        await unitOfWork.BeginTransactionAsync();
        try
        {
            // Yazı değişmeden önce çalışmalı: aksi halde eski ve yeni öne çıkan aynı SaveChanges'e düşer
            await ReleaseFeaturedAsync(request.CategoryId, request.IsFeatured, request.Id);

            oldUploads = UploadPaths.Of(post);
            mapper.Map(request, post);
            await SetTagsAsync(post, request.Tags);

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }

        await uploadCleanup.DeleteUnusedAsync(oldUploads.Except(UploadPaths.Of(post)));

        logger.LogInformation("Blog yazısı {Id} güncellendi", post.Id);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var post = await unitOfWork.BlogPosts.GetByIdAsync(id);
        if (post is null)
            return Result.NotFound(PostNotFound);

        unitOfWork.BlogPosts.Remove(post);
        await unitOfWork.SaveChangesAsync();
        await uploadCleanup.DeleteUnusedAsync(UploadPaths.Of(post));

        logger.LogInformation("Blog yazısı {Id} silindi", id);
        return Result.Success();
    }

    public async Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids)
    {
        var loaded = await BulkDelete.LoadAsync(ids, unitOfWork.BlogPosts.GetByIdAsync, "Seçilen yazılardan biri bulunamadı.");
        if (!loaded.IsSuccess)
            return loaded;

        foreach (var post in loaded.Value!)
            unitOfWork.BlogPosts.Remove(post);
        await unitOfWork.SaveChangesAsync();
        await uploadCleanup.DeleteUnusedAsync(loaded.Value!.SelectMany(UploadPaths.Of));

        logger.LogInformation("{Count} blog yazısı silindi: {Ids}", loaded.Value!.Count, loaded.Value!.Select(p => p.Id));
        return Result.Success();
    }

    public async Task<Result> PublishAsync(int id)
    {
        var post = await unitOfWork.BlogPosts.GetByIdAsync(id);
        if (post is null)
            return Result.NotFound(PostNotFound);

        post.IsPublished = true;
        post.PublishedAt ??= DateTime.UtcNow;
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Blog yazısı {Id} yayınlandı", id);
        return Result.Success();
    }

    public async Task<Result> UnpublishAsync(int id)
    {
        var post = await unitOfWork.BlogPosts.GetByIdAsync(id);
        if (post is null)
            return Result.NotFound(PostNotFound);

        post.IsPublished = false;
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Blog yazısı {Id} yayından kaldırıldı", id);
        return Result.Success();
    }

    // "Öne Çıkan" her kategoride tektir: yazı işaretlendiyse aynı kategorideki eskisi kaldırılır.
    // Unique index ihlal edilmesin diye eskisi önce ayrı SaveChanges ile kaydedilir (çağıran transaction açar).
    private async Task ReleaseFeaturedAsync(int categoryId, bool isFeatured, int? currentId)
    {
        if (!isFeatured)
            return;

        var previous = await unitOfWork.BlogPosts.GetFeaturedAsync(categoryId);
        if (previous is null || previous.Id == currentId)
            return;

        previous.IsFeatured = false;
        await unitOfWork.SaveChangesAsync();
    }

    // Etiket bağlantıları gönderilen listeyle değiştirilir; olmayan etiket yeni oluşturulur.
    private async Task SetTagsAsync(BlogPost post, List<string> tagNames)
    {
        var wanted = tagNames
            .Select(name => (Name: name.Trim().TrimStart('#'), Slug: SlugHelper.ToSlug(name)))
            .Where(t => t.Slug.Length > 0)
            .DistinctBy(t => t.Slug)
            .ToList();
        var wantedSlugs = wanted.Select(t => t.Slug).ToHashSet();

        // Listeden çıkan bağlantılar silinir (aynı bağlantıyı silip yeniden eklemek EF'de takip çakışması yaratır)
        foreach (var link in post.Tags.Where(l => !wantedSlugs.Contains(l.BlogTag.Slug)).ToList())
            post.Tags.Remove(link);

        var linkedSlugs = post.Tags.Select(l => l.BlogTag.Slug).ToHashSet();
        var missing = wanted.Where(t => !linkedSlugs.Contains(t.Slug)).ToList();
        if (missing.Count == 0)
            return;

        var existing = (await unitOfWork.BlogPosts.GetTagsBySlugsAsync(missing.Select(t => t.Slug).ToList()))
            .ToDictionary(t => t.Slug);

        foreach (var (name, slug) in missing)
        {
            var tag = existing.GetValueOrDefault(slug) ?? new BlogTag { Name = name, Slug = slug };
            post.Tags.Add(new BlogPostTag { BlogPost = post, BlogTag = tag });
        }
    }

    private async Task<List<string>> CheckReferencesAsync(CreateBlogPostRequest request)
    {
        var errors = new List<string>();

        if (await unitOfWork.BlogCategories.GetByIdAsync(request.CategoryId) is null)
            errors.Add("Kategori bulunamadı.");

        if (request.RecommendedPackageId is { } packageId && await unitOfWork.Packages.GetByIdAsync(packageId) is null)
            errors.Add("Önerilen paket bulunamadı.");

        if (request.AuthorId is { } authorId && await userManager.FindByIdAsync(authorId.ToString()) is null)
            errors.Add("Yazar bulunamadı.");

        return errors;
    }
}
