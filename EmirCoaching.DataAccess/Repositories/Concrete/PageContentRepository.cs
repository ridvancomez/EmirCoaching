using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class PageContentRepository(AppDbContext context)
    : Repository<PageContent>(context), IPageContentRepository
{
    public async Task<PageContent?> GetByPageKeyAsync(string pageKey) =>
        await Set.AsNoTracking().FirstOrDefaultAsync(x => x.PageKey == pageKey);
}
