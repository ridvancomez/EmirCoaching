using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.Entities.Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmirCoaching.DataAccess.Repositories.UnitOfWork;

// DbContext'in ömrünü DI yönetir; burada yalnızca transaction kapatılır.
// IDisposable da uygulanır: senkron kapatılan DI scope'ları yalnızca IAsyncDisposable olan servislerde hata fırlatır.
public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
{
    private readonly Dictionary<Type, object> _repositories = [];
    private IDbContextTransaction? _transaction;

    public IPackageRepository Packages => Get(() => new PackageRepository(context));
    public IBlogPostRepository BlogPosts => Get(() => new BlogPostRepository(context));
    public ITransformationRepository Transformations => Get(() => new TransformationRepository(context));
    public IFaqRepository Faqs => Get(() => new FaqRepository(context));
    public ILegalDocumentRepository LegalDocuments => Get(() => new LegalDocumentRepository(context));
    public IContactMessageRepository ContactMessages => Get(() => new ContactMessageRepository(context));
    public IDietFormRepository DietForms => Get(() => new DietFormRepository(context));
    public INewsletterRepository NewsletterSubscribers => Get(() => new NewsletterRepository(context));

    public IRepository<T> Repository<T>() where T : BaseEntity =>
        Get<IRepository<T>>(() => new Repository<T>(context));

    private TRepository Get<TRepository>(Func<TRepository> factory) where TRepository : class
    {
        if (!_repositories.TryGetValue(typeof(TRepository), out var repository))
        {
            repository = factory();
            _repositories[typeof(TRepository)] = repository;
        }
        return (TRepository)repository;
    }

    public Task<int> SaveChangesAsync() => context.SaveChangesAsync();

    public async Task BeginTransactionAsync()
    {
        if (_transaction is not null)
            throw new InvalidOperationException("Zaten açık bir transaction var.");

        _transaction = await context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_transaction is null)
            throw new InvalidOperationException("Açık transaction yok.");

        try
        {
            await _transaction.CommitAsync();
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction is null)
            return;

        try
        {
            await _transaction.RollbackAsync();
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeTransactionAsync();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _transaction = null;
        GC.SuppressFinalize(this);
    }
}
