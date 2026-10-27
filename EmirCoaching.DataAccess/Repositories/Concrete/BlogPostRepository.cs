using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class BlogPostRepository(AppDbContext context) : SlugRepository<BlogPost>(context), IBlogPostRepository
{
    private IQueryable<BlogPost> Published =>
        Set.AsNoTracking().Where(x => x.IsPublished && x.PublishedAt <= DateTime.UtcNow);

    private static IQueryable<BlogPost> InCategory(IQueryable<BlogPost> query, string? categorySlug) =>
        query.WhereIf(!string.IsNullOrWhiteSpace(categorySlug), x => x.Category.Slug == categorySlug);

    public Task<PagedList<BlogPost>> GetPublishedPagedAsync(string? categorySlug, int page, int size) =>
        InCategory(Published.Include(x => x.Category), categorySlug)
            .OrderByDescending(x => x.PublishedAt)
            .ToPagedListAsync(page, size);

    public async Task<BlogPost?> GetBySlugAsync(string slug) =>
        await WithDetails(Published).FirstOrDefaultAsync(x => x.Slug == slug);

    public Task<PagedList<BlogPost>> GetPagedAsync(int? categoryId, bool? isPublished, int page, int size) =>
        Set.AsNoTracking()
            .Include(x => x.Category)
            .WhereIf(categoryId is not null, x => x.CategoryId == categoryId)
            .WhereIf(isPublished is not null, x => x.IsPublished == isPublished)
            .NewestFirst()
            .ToPagedListAsync(page, size);

    public async Task<BlogPost?> GetDetailByIdAsync(int id) =>
        await WithDetails(Set.AsNoTracking()).FirstOrDefaultAsync(x => x.Id == id);

    // Detay sayfası ve admin formu için ilişkiler
    private static IQueryable<BlogPost> WithDetails(IQueryable<BlogPost> query) =>
        query
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.RecommendedPackage)
                .ThenInclude(p => p!.Highlights.OrderBy(h => h.SortOrder))
            .Include(x => x.Tags)
                .ThenInclude(t => t.BlogTag)
            .AsSplitQuery();

    public async Task<IReadOnlyList<BlogPost>> GetRelatedAsync(int id, int count)
    {
        var categoryId = await Set.Where(x => x.Id == id).Select(x => (int?)x.CategoryId).FirstOrDefaultAsync();
        if (categoryId is null)
            return [];

        return await Published
            .Include(x => x.Category)
            .Where(x => x.CategoryId == categoryId && x.Id != id)
            .OrderByDescending(x => x.PublishedAt)
            .Take(count)
            .ToListAsync();
    }

    // İşaretli yazı önce gelir; işaretli yoksa en son yayınlanan
    public async Task<BlogPost?> GetFeaturedForDisplayAsync(string? categorySlug) =>
        await InCategory(Published.Include(x => x.Category).Include(x => x.Author), categorySlug)
            .OrderByDescending(x => x.IsFeatured)
            .ThenByDescending(x => x.PublishedAt)
            .FirstOrDefaultAsync();

    public async Task<BlogPost?> GetFeaturedAsync(int categoryId) =>
        await Set.FirstOrDefaultAsync(x => x.CategoryId == categoryId && x.IsFeatured);

    public async Task<BlogPost?> GetByIdWithTagsAsync(int id) =>
        await Set.Include(x => x.Tags).ThenInclude(t => t.BlogTag).FirstOrDefaultAsync(x => x.Id == id);

    public async Task<List<BlogTag>> GetTagsBySlugsAsync(IReadOnlyCollection<string> slugs) =>
        await Context.BlogTags.Where(x => slugs.Contains(x.Slug)).ToListAsync();

    public Task<bool> AnyInCategoryAsync(int categoryId) =>
        Set.AnyAsync(x => x.CategoryId == categoryId);
}
