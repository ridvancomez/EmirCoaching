using System.Linq.Expressions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Extensions;

// Repository'lerde tekrar eden sorgu parçaları
public static class QueryableExtensions
{
    private const int MaxPageSize = 100;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate) =>
        condition ? query.Where(predicate) : query;

    public static IQueryable<T> ActiveOnly<T>(this IQueryable<T> query) where T : IActivatable =>
        query.Where(x => x.IsActive);

    // Aynı SortOrder'da eklenme sırası korunur
    public static IOrderedQueryable<T> Ordered<T>(this IQueryable<T> query) where T : BaseEntity, ISortable =>
        query.OrderBy(x => x.SortOrder).ThenBy(x => x.Id);

    public static IOrderedQueryable<T> NewestFirst<T>(this IQueryable<T> query) where T : BaseEntity =>
        query.OrderByDescending(x => x.CreatedAt);

    public static Task<bool> SlugExistsAsync<T>(this IQueryable<T> query, string slug, int? excludeId)
        where T : BaseEntity, ISluggable =>
        query.AnyAsync(x => x.Slug == slug && x.Id != excludeId);

    public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int page, int size)
    {
        page = Math.Max(page, 1);
        size = Math.Clamp(size, 1, MaxPageSize);

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * size).Take(size).ToListAsync();
        return new PagedList<T>(items, total, page, size);
    }
}
