using EmirCoaching.Web.Api;
using Microsoft.Extensions.Caching.Memory;

namespace EmirCoaching.Web.Services;

/// <summary>Her sayfada kullanılan ortak veri: header, footer ve iletişim modalı.</summary>
public sealed record LayoutData(
    SiteSettingResponse Settings,
    IReadOnlyList<PackageResponse> Packages,
    IReadOnlyList<SocialLinkResponse> SocialLinks)
{
    /// <summary>Menü, footer ve formlardaki paket listesi: salondaki paketler (her paketin evde karşılığı aynı adı taşır).</summary>
    public IEnumerable<PackageResponse> MenuPackages => Packages.Where(p => p.Location == PackageLocation.Salonda);

    public string WhatsAppUrl => Link(SocialPlatform.WhatsApp) ?? $"https://wa.me/{new string(Settings.Phone.Where(char.IsDigit).ToArray())}";

    public string? Link(SocialPlatform platform) => SocialLinks.FirstOrDefault(l => l.Platform == platform)?.Url;
}

/// <summary>
/// Sitenin API'ye tek kapısı. Yanıtlar kısa süre (Api:CacheSeconds) bellekte tutulur: her sayfa birkaç uç çağırır,
/// paneldeki değişiklik en geç bu süre sonra görünür. Bulunamayan kayıt (404) null döner ve önbelleğe alınmaz:
/// adresteki slug'la üretilen anahtarlar yalnızca var olan kayıtlarla sınırlı kalır.
/// Anahtarlar metot başına ayrı önekle başlar; slug başka bir metodun anahtarıyla çakışamaz.
/// </summary>
public sealed class SiteApi(
    IMemoryCache cache,
    IConfiguration configuration,
    ISiteSettingsClient siteSettings,
    IPackagesClient packages,
    ISocialLinksClient socialLinks,
    ISocialPostsClient socialPosts,
    IPageContentsClient pageContents,
    IPurchaseStepsClient purchaseSteps,
    ICertificatesClient certificates,
    ICoachProfileClient coachProfile,
    ITestimonialsClient testimonials,
    IFaqsClient faqs,
    ITransformationsClient transformations,
    IBlogPostsClient blogPosts,
    IBlogCategoriesClient blogCategories,
    ILegalDocumentsClient legalDocuments,
    IDietProgramClient dietProgram,
    IDietProgramStepsClient dietProgramSteps,
    ISiteStatsClient siteStats)
{
    private readonly TimeSpan _ttl = TimeSpan.FromSeconds(configuration.GetValue("Api:CacheSeconds", 30));

    public Task<LayoutData> GetLayoutAsync() => CachedAsync("layout", async () =>
    {
        var settingsTask = siteSettings.GetAsync();
        var packagesTask = packages.GetListAsync();
        var linksTask = socialLinks.GetActiveAsync();
        return new LayoutData(await settingsTask, await packagesTask, await linksTask);
    });

    public async Task<string> GetWhatsAppUrlAsync() => (await GetLayoutAsync()).WhatsAppUrl;

    public Task<PackageDetailResponse?> GetPackageAsync(string slug) =>
        CachedAsync($"package-detail:{slug}", () => OrNullAsync(() => packages.GetDetailAsync(slug)));

    public Task<PageContentResponse?> GetPageContentAsync(string pageKey) =>
        CachedAsync($"page-content:{pageKey}", () => OrNullAsync(() => pageContents.GetByPageKeyAsync(pageKey)));

    public Task<List<PurchaseStepResponse>> GetPurchaseStepsAsync() => CachedAsync("purchase-steps", purchaseSteps.GetAllAsync);

    public Task<List<CertificateResponse>> GetCertificatesAsync() => CachedAsync("certificates", certificates.GetActiveAsync);

    public Task<CoachProfileResponse?> GetCoachProfileAsync() =>
        CachedAsync("coach", () => OrNullAsync(coachProfile.GetAsync));

    public Task<List<TestimonialResponse>> GetHomeTestimonialsAsync() => CachedAsync("testimonials:home", testimonials.GetHomeAsync);

    public Task<List<FaqResponse>> GetFaqsAsync(FaqGroup group) => CachedAsync($"faqs:{group}", () => faqs.GetByGroupAsync(group));

    public Task<List<SocialPostResponse>> GetSocialPostsAsync(SocialPlatform platform, int count) =>
        CachedAsync($"social:{platform}:{count}", () => socialPosts.GetActiveByPlatformAsync(platform, count));

    public Task<List<TransformationResponse>> GetHomeTransformationsAsync() =>
        CachedAsync("transformations:home", transformations.GetHomeAsync);

    /// <summary>Değişimler sayfası: kartın "Hikayeyi Oku" penceresi uzun hikâyeyi de ister, o yüzden detaylar çekilir.</summary>
    public Task<List<TransformationDetailResponse>> GetTransformationDetailsAsync() => CachedAsync("transformations:all", async () =>
    {
        var list = await transformations.GetListAsync(null, null);
        var details = await Task.WhenAll(list.Select(t => transformations.GetDetailAsync(t.Id)));
        return details.ToList();
    });

    public Task<PagedResultOfBlogPostResponse> GetBlogPostsAsync(int page, int size) =>
        CachedAsync($"blog-list:{page}:{size}", () => blogPosts.GetPagedAsync(null, page, size));

    public Task<BlogPostResponse?> GetFeaturedPostAsync() =>
        CachedAsync("blog-featured", () => OrNullAsync(() => blogPosts.GetFeaturedAsync(null)));

    // API'de /blog-posts/featured sabit rota; bu slug detay değil öne çıkan yazının özetini döndürür
    private const string ReservedBlogSlug = "featured";

    public Task<BlogPostDetailResponse?> GetBlogPostAsync(string slug) =>
        slug == ReservedBlogSlug
            ? Task.FromResult<BlogPostDetailResponse?>(null)
            : CachedAsync($"blog-post:{slug}", () => OrNullAsync(() => blogPosts.GetDetailAsync(slug)));

    public Task<List<BlogCategoryResponse>> GetBlogCategoriesAsync() => CachedAsync("blog-categories", blogCategories.GetAllAsync);

    public Task<LegalDocumentResponse?> GetLegalDocumentAsync(LegalDocumentType type) =>
        CachedAsync($"legal:{type}", () => OrNullAsync(() => legalDocuments.GetCurrentAsync(type)));

    public Task<List<LegalDocumentRevisionResponse>> GetLegalRevisionsAsync(LegalDocumentType type) =>
        CachedAsync($"legal-revisions:{type}", () => legalDocuments.GetRevisionsAsync(type));

    public Task<DietProgramResponse?> GetDietProgramAsync() =>
        CachedAsync("diet-program", () => OrNullAsync(dietProgram.GetActiveAsync));

    public Task<List<DietProgramStepResponse>> GetDietProgramStepsAsync() => CachedAsync("diet-steps", dietProgramSteps.GetAllAsync);

    public Task<List<SiteStatResponse>> GetSiteStatsAsync() => CachedAsync("site-stats", siteStats.GetAllAsync);

    private async Task<T> CachedAsync<T>(string key, Func<Task<T>> factory)
    {
        if (cache.TryGetValue(key, out T? value))
            return value!;

        value = await factory();
        if (value is not null)
            cache.Set(key, value, _ttl);
        return value;
    }

    private static async Task<T?> OrNullAsync<T>(Func<Task<T>> call) where T : class
    {
        try
        {
            return await call();
        }
        catch (ApiException e) when (e.StatusCode == StatusCodes.Status404NotFound)
        {
            return null;
        }
    }
}
