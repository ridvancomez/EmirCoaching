using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class TransformationRepository(AppDbContext context)
    : Repository<Transformation>(context), ITransformationRepository
{
    private IQueryable<Transformation> WithMetrics =>
        Set.AsNoTracking().Include(x => x.Metrics.OrderBy(m => m.SortOrder));

    public async Task<IReadOnlyList<Transformation>> GetListAsync(TransformationCategory? category, TransformationSort sort)
    {
        var query = WithMetrics;

        if (category is not null)
            query = query.Where(x => x.Category == category);

        query = sort == TransformationSort.EnEskiler
            ? query.OrderBy(x => x.Year).ThenBy(x => x.SortOrder)
            : query.OrderByDescending(x => x.Year).ThenBy(x => x.SortOrder);

        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<Transformation>> GetForHomeAsync() =>
        await WithMetrics
            .Where(x => x.ShowOnHome)
            .OrderBy(x => x.SortOrder)
            .ToListAsync();

    public async Task<Transformation?> GetWithMetricsAsync(int id) =>
        await Set.Include(x => x.Metrics).FirstOrDefaultAsync(x => x.Id == id);
}
