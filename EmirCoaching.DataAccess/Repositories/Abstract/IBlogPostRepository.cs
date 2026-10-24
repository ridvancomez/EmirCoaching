using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IBlogPostRepository : IRepository<BlogPost>
{
    Task<PagedList<BlogPost>> GetPublishedPagedAsync(string? categorySlug, int page, int size);
    Task<BlogPost?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<BlogPost>> GetRelatedAsync(int id, int count);
    Task<bool> SlugExistsAsync(string slug, int? excludeId = null);
}
