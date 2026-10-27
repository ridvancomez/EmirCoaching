using EmirCoaching.Entities.Entities;

namespace EmirCoaching.DataAccess.Repositories.Abstract;

public interface IPageContentRepository : IRepository<PageContent>
{
    // Sayfanın hero/breadcrumb içeriği ("home", "paketler", "blog" ...)
    Task<PageContent?> GetByPageKeyAsync(string pageKey);
}
