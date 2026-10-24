using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.DataAccess.Repositories.Concrete;

public class NewsletterRepository(AppDbContext context)
    : Repository<NewsletterSubscriber>(context), INewsletterRepository
{
    public async Task<NewsletterSubscriber?> GetByEmailAsync(string email) =>
        await Set.FirstOrDefaultAsync(x => x.Email == email);
}
