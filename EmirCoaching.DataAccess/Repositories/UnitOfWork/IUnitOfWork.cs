using EmirCoaching.DataAccess.Repositories.Abstract;

namespace EmirCoaching.DataAccess.Repositories.UnitOfWork;

// Kural: her entity için I{Entity}Repository; property adı AppDbContext'teki DbSet adıyla aynı.
// Repository'si olmayanlar üst kaydı üzerinden yönetilir:
// PackageModuleItem → PackageModule (cascade), BlogTag/BlogPostTag → BlogPosts, LegalDocumentRevision → LegalDocuments.
public interface IUnitOfWork : IAsyncDisposable
{
    // Paketler
    IPackageRepository Packages { get; }
    IPackageModuleRepository PackageModules { get; }
    IPackageHighlightRepository PackageHighlights { get; }
    ITestimonialRepository Testimonials { get; }

    // Blog
    IBlogCategoryRepository BlogCategories { get; }
    IBlogPostRepository BlogPosts { get; }

    // Değişimler
    ITransformationTypeRepository TransformationTypes { get; }
    ITransformationRepository Transformations { get; }
    ITransformationMetricRepository TransformationMetrics { get; }

    // Site içeriği
    IFaqRepository Faqs { get; }
    ISiteStatRepository SiteStats { get; }
    ISiteSettingRepository SiteSettings { get; }
    ISocialLinkRepository SocialLinks { get; }
    ISocialPostRepository SocialPosts { get; }
    ICertificateRepository Certificates { get; }
    IPageContentRepository PageContents { get; }
    IDietProgramRepository DietPrograms { get; }
    IDietProgramStepRepository DietProgramSteps { get; }
    IPurchaseStepRepository PurchaseSteps { get; }
    ILegalDocumentRepository LegalDocuments { get; }

    // Formlar ve onaylar
    IContactMessageRepository ContactMessages { get; }
    IDietFormSubmissionRepository DietFormSubmissions { get; }
    INewsletterSubscriberRepository NewsletterSubscribers { get; }
    IConsentRecordRepository ConsentRecords { get; }

    // Kimlik doğrulama (kullanıcılar UserManager ile yönetilir)
    IRefreshTokenRepository RefreshTokens { get; }
    ILoginAttemptRepository LoginAttempts { get; }

    // Entity'ye bağlı olmayan sorgu: yüklenen dosyaları kullanan kayıtlar (görsel temizliği)
    IUploadReferenceRepository UploadReferences { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}
