using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public abstract class SortedRepository<T>(AppDbContext context) : Repository<T>(context), ISortedRepository<T>
    where T : BaseEntity, ISortable
{
    public async Task<IReadOnlyList<T>> GetOrderedAsync() =>
        await Set.AsNoTracking().Ordered().ToListAsync();
}
