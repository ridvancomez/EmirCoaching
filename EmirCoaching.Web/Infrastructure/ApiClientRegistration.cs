using EmirCoaching.Web.Api;

namespace EmirCoaching.Web.Infrastructure;

public static class ApiClientRegistration
{
    /// <summary>Sitenin kullandığı (herkese açık) API istemcilerini aynı taban adresle kaydeder.</summary>
    public static IServiceCollection AddApiClients(this IServiceCollection services, Uri baseUrl)
    {
        Add<ISiteSettingsClient, SiteSettingsClient>();
        Add<IPackagesClient, PackagesClient>();
        Add<ISocialLinksClient, SocialLinksClient>();
        Add<ISocialPostsClient, SocialPostsClient>();
        Add<IPageContentsClient, PageContentsClient>();
        Add<IPurchaseStepsClient, PurchaseStepsClient>();
        Add<ICertificatesClient, CertificatesClient>();
        Add<ICoachProfileClient, CoachProfileClient>();
        Add<ITestimonialsClient, TestimonialsClient>();
        Add<IFaqsClient, FaqsClient>();
        Add<ITransformationsClient, TransformationsClient>();
        Add<IBlogPostsClient, BlogPostsClient>();
        Add<IBlogCategoriesClient, BlogCategoriesClient>();
        Add<ILegalDocumentsClient, LegalDocumentsClient>();
        Add<IDietProgramClient, DietProgramClient>();
        Add<IDietProgramStepsClient, DietProgramStepsClient>();
        Add<ISiteStatsClient, SiteStatsClient>();
        return services;

        void Add<TClient, TImplementation>() where TClient : class where TImplementation : class, TClient =>
            services.AddHttpClient<TClient, TImplementation>(client =>
            {
                client.BaseAddress = baseUrl;
                client.Timeout = TimeSpan.FromSeconds(10);
            });
    }
}
