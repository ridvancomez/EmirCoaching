using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public abstract class SlugRepository<T>(AppDbContext context) : Repository<T>(context), ISlugRepository<T>
    where T : BaseEntity, ISluggable
{
    public Task<bool> SlugExistsAsync(string slug, int? excludeId = null) =>
        Set.SlugExistsAsync(slug, excludeId);
}
