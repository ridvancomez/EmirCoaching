using EmirCoaching.DataAccess.Context;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.Concrete;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmirCoaching.DataAccess.Repositories.UnitOfWork;

// DbContext'in ömrünü DI yönetir; burada yalnızca transaction kapatılır.
// IDisposable da uygulanır: senkron kapatılan DI scope'ları yalnızca IAsyncDisposable olan servislerde hata fırlatır.
public class UnitOfWork(AppDbContext context) : IUnitOfWork, IDisposable
{
    private readonly Dictionary<Type, object> _repositories = [];
    private IDbContextTransaction? _transaction;

    public IPackageRepository Packages => Get(() => new PackageRepository(context));
    public IPackageModuleRepository PackageModules => Get(() => new PackageModuleRepository(context));
    public IPackageHighlightRepository PackageHighlights => Get(() => new PackageHighlightRepository(context));
    public ITestimonialRepository Testimonials => Get(() => new TestimonialRepository(context));

    public IBlogCategoryRepository BlogCategories => Get(() => new BlogCategoryRepository(context));
    public IBlogPostRepository BlogPosts => Get(() => new BlogPostRepository(context));

    public ITransformationTypeRepository TransformationTypes => Get(() => new TransformationTypeRepository(context));
    public ITransformationRepository Transformations => Get(() => new TransformationRepository(context));
    public ITransformationMetricRepository TransformationMetrics => Get(() => new TransformationMetricRepository(context));

    public IFaqRepository Faqs => Get(() => new FaqRepository(context));
    public ISiteStatRepository SiteStats => Get(() => new SiteStatRepository(context));
    public ISiteSettingRepository SiteSettings => Get(() => new SiteSettingRepository(context));
    public ISocialLinkRepository SocialLinks => Get(() => new SocialLinkRepository(context));
    public ISocialPostRepository SocialPosts => Get(() => new SocialPostRepository(context));
    public ICertificateRepository Certificates => Get(() => new CertificateRepository(context));
    public IPageContentRepository PageContents => Get(() => new PageContentRepository(context));
    public IDietProgramRepository DietPrograms => Get(() => new DietProgramRepository(context));
    public IDietProgramStepRepository DietProgramSteps => Get(() => new DietProgramStepRepository(context));
    public IPurchaseStepRepository PurchaseSteps => Get(() => new PurchaseStepRepository(context));
    public ILegalDocumentRepository LegalDocuments => Get(() => new LegalDocumentRepository(context));

    public IContactMessageRepository ContactMessages => Get(() => new ContactMessageRepository(context));
    public IDietFormSubmissionRepository DietFormSubmissions => Get(() => new DietFormSubmissionRepository(context));
    public INewsletterSubscriberRepository NewsletterSubscribers => Get(() => new NewsletterSubscriberRepository(context));
    public IConsentRecordRepository ConsentRecords => Get(() => new ConsentRecordRepository(context));

    public IRefreshTokenRepository RefreshTokens => Get(() => new RefreshTokenRepository(context));
    public ILoginAttemptRepository LoginAttempts => Get(() => new LoginAttemptRepository(context));

    public IUploadReferenceRepository UploadReferences => Get(() => new UploadReferenceRepository(context));

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
