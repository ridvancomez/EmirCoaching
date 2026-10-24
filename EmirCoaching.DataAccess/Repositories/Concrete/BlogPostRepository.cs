using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class BlogPostRepository(AppDbContext context) : Repository<BlogPost>(context), IBlogPostRepository
{
    private IQueryable<BlogPost> Published =>
        Set.AsNoTracking().Where(x => x.IsPublished && x.PublishedAt <= DateTime.UtcNow);

    public Task<PagedList<BlogPost>> GetPublishedPagedAsync(string? categorySlug, int page, int size)
    {
        var query = Published.Include(x => x.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(categorySlug))
            query = query.Where(x => x.Category.Slug == categorySlug);

        return ToPagedListAsync(query.OrderByDescending(x => x.PublishedAt), page, size);
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug) =>
        await Published
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.RecommendedPackage)
                .ThenInclude(p => p!.Highlights.OrderBy(h => h.SortOrder))
            .FirstOrDefaultAsync(x => x.Slug == slug);

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

    public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null) =>
        await Set.AnyAsync(x => x.Slug == slug && x.Id != excludeId);
}
