using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Extensions;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class NewsletterSubscriberRepository(AppDbContext context)
    : PersonalDataRepository<NewsletterSubscriber>(context), INewsletterSubscriberRepository
{
    protected override IQueryable<ConsentRecord> ConsentsOf(int id) =>
        Context.ConsentRecords.Where(c => c.NewsletterSubscriberId == id);

    public async Task<NewsletterSubscriber?> GetByEmailAsync(string email) =>
        await Set.FirstOrDefaultAsync(x => x.Email == email);

    public Task<int> CountByIpSinceAsync(string ipAddress, DateTime since) =>
        Set.CountAsync(x => x.IpAddress == ipAddress && x.CreatedAt >= since);

    public Task<PagedList<NewsletterSubscriber>> GetPagedAsync(int page, int size) =>
        Set.AsNoTracking().NewestFirst().ToPagedListAsync(page, size);
}
