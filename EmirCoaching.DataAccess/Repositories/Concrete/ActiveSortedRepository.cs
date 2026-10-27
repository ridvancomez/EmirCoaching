using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public abstract class ActiveSortedRepository<T>(AppDbContext context) : SortedRepository<T>(context), IActiveSortedRepository<T>
    where T : BaseEntity, ISortable, IActivatable
{
    public async Task<IReadOnlyList<T>> GetActiveOrderedAsync() =>
        await Set.AsNoTracking().ActiveOnly().Ordered().ToListAsync();
}
