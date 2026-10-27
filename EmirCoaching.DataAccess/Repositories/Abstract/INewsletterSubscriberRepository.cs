using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface INewsletterSubscriberRepository : IPersonalDataRepository<NewsletterSubscriber>
{
    Task<NewsletterSubscriber?> GetByEmailAsync(string email);
    Task<int> CountByIpSinceAsync(string ipAddress, DateTime since);

    // Admin listesi: yeniden eskiye
    Task<PagedList<NewsletterSubscriber>> GetPagedAsync(int page, int size);
}
