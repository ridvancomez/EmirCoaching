using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class TransformationTypeRepository(AppDbContext context)
    : SortedRepository<TransformationType>(context), ITransformationTypeRepository
{
    public Task<bool> SlugExistsAsync(string slug, int? excludeId = null) =>
        Set.SlugExistsAsync(slug, excludeId);
}
