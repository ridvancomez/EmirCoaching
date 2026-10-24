using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.DataAccess.Repositories.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    IPackageRepository Packages { get; }
    IBlogPostRepository BlogPosts { get; }
    ITransformationRepository Transformations { get; }
    IFaqRepository Faqs { get; }
    ILegalDocumentRepository LegalDocuments { get; }
    IContactMessageRepository ContactMessages { get; }
    IDietFormRepository DietForms { get; }
    INewsletterRepository NewsletterSubscribers { get; }

    // Özel arayüzü olmayan entity'ler için (SiteSetting, SiteStat, BlogCategory, ConsentRecord ...)
    IRepository<T> Repository<T>() where T : BaseEntity;

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
