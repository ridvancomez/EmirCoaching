using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class FaqRepository(AppDbContext context) : Repository<Faq>(context), IFaqRepository
{
    public async Task<IReadOnlyList<Faq>> GetByGroupAsync(FaqGroup group) =>
        await Set.AsNoTracking()
            .Where(x => x.Group == group)
            .OrderBy(x => x.SortOrder)
            .ToListAsync();
}
