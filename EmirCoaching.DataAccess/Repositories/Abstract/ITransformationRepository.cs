using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface ITransformationRepository : IRepository<Transformation>
{
    Task<IReadOnlyList<Transformation>> GetListAsync(TransformationCategory? category, TransformationSort sort);
    Task<IReadOnlyList<Transformation>> GetForHomeAsync();
    Task<Transformation?> GetWithMetricsAsync(int id);
}
