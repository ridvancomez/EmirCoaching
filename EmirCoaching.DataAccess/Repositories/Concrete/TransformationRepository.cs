using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class TransformationRepository(AppDbContext context)
    : Repository<Transformation>(context), ITransformationRepository
{
    private IQueryable<Transformation> WithDetails =>
        Set.AsNoTracking()
            .Include(x => x.TransformationType)
            .Include(x => x.Metrics.OrderBy(m => m.SortOrder));

    public async Task<IReadOnlyList<Transformation>> GetListAsync(string? typeSlug, TransformationSort sort)
    {
        var query = WithDetails.ActiveOnly().WhereIf(!string.IsNullOrWhiteSpace(typeSlug), x => x.TransformationType.Slug == typeSlug);

        query = sort == TransformationSort.EnEskiler
            ? query.OrderBy(x => x.Year).ThenBy(x => x.SortOrder)
            : query.OrderByDescending(x => x.Year).ThenBy(x => x.SortOrder);

        return await query.ToListAsync();
    }

    // Ana sayfada işaretli değişimlerin tamamı, yeniden eskiye
    public async Task<IReadOnlyList<Transformation>> GetForHomeAsync() =>
        await WithDetails
            .ActiveOnly()
            .Where(x => x.ShowOnHome)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

    public async Task<IReadOnlyList<Transformation>> GetOrderedListAsync() =>
        await WithDetails
            .OrderByDescending(x => x.Year).ThenBy(x => x.SortOrder)
            .ToListAsync();

    public async Task<Transformation?> GetWithMetricsAsync(int id) =>
        await Set.Include(x => x.TransformationType).Include(x => x.Metrics).FirstOrDefaultAsync(x => x.Id == id);

    public Task<bool> AnyWithTypeAsync(int transformationTypeId) =>
        Set.AnyAsync(x => x.TransformationTypeId == transformationTypeId);
}
