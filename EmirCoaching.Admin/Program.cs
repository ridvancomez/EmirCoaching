using EmirCoaching.Admin;
using EmirCoaching.Admin.Api;
using EmirCoaching.Admin.Auth;
using EmirCoaching.Admin.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Panel API'yi çağırır; adres wwwroot/appsettings(.Development).json → ApiBaseUrl
var apiBaseUrl = new Uri(builder.Configuration["ApiBaseUrl"]
    ?? throw new InvalidOperationException("ApiBaseUrl ayarı eksik."));

// "Sitede gör" bağlantıları → SiteBaseUrl
SiteLinks.SiteBaseUrl = builder.Configuration["SiteBaseUrl"] ?? "/";

// Oturum. WebAssembly'de scoped = uygulama boyunca tek örnek.
builder.Services.AddScoped<AuthSession>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped(sp => new Lazy<AuthService>(sp.GetRequiredService<AuthService>));
builder.Services.AddScoped(_ => new AuthHttpClient(
    new HttpClient(new IncludeCredentialsHandler { InnerHandler = new HttpClientHandler() }) { BaseAddress = apiBaseUrl }));

// Diğer tüm istekler: Bearer token → cookie → tarayıcı
builder.Services.AddScoped(sp => new HttpClient(
    new BearerTokenHandler(sp.GetRequiredService<AuthSession>(), sp.GetRequiredService<Lazy<AuthService>>())
    {
        InnerHandler = new IncludeCredentialsHandler { InnerHandler = new HttpClientHandler() }
    })
{ BaseAddress = apiBaseUrl });

// NSwag'in ürettiği istemciler (OpenApi/emircoaching-api.json); sayfalar arayüzü enjekte eder
builder.Services.AddScoped<IAdminBlogCategoriesClient, AdminBlogCategoriesClient>();
builder.Services.AddScoped<IAdminBlogPostsClient, AdminBlogPostsClient>();
builder.Services.AddScoped<IAdminCertificatesClient, AdminCertificatesClient>();
builder.Services.AddScoped<IAdminContactMessagesClient, AdminContactMessagesClient>();
builder.Services.AddScoped<IAdminDietFormSubmissionsClient, AdminDietFormSubmissionsClient>();
builder.Services.AddScoped<IAdminDietProgramClient, AdminDietProgramClient>();
builder.Services.AddScoped<IAdminDietProgramStepsClient, AdminDietProgramStepsClient>();
builder.Services.AddScoped<IAdminFaqsClient, AdminFaqsClient>();
builder.Services.AddScoped<IAdminLegalDocumentsClient, AdminLegalDocumentsClient>();
builder.Services.AddScoped<IAdminLoginAttemptsClient, AdminLoginAttemptsClient>();
builder.Services.AddScoped<IAdminNewsletterSubscribersClient, AdminNewsletterSubscribersClient>();
builder.Services.AddScoped<IAdminPackagesClient, AdminPackagesClient>();
builder.Services.AddScoped<IAdminPageContentsClient, AdminPageContentsClient>();
builder.Services.AddScoped<IAdminPurchaseStepsClient, AdminPurchaseStepsClient>();
builder.Services.AddScoped<IAdminSiteSettingsClient, AdminSiteSettingsClient>();
builder.Services.AddScoped<IAdminSiteStatsClient, AdminSiteStatsClient>();
builder.Services.AddScoped<IAdminSocialLinksClient, AdminSocialLinksClient>();
builder.Services.AddScoped<IAdminSocialPostsClient, AdminSocialPostsClient>();
builder.Services.AddScoped<IAdminTestimonialsClient, AdminTestimonialsClient>();
builder.Services.AddScoped<IAdminTransformationsClient, AdminTransformationsClient>();
builder.Services.AddScoped<IAdminTransformationTypesClient, AdminTransformationTypesClient>();
builder.Services.AddScoped<IAuthClient, AuthClient>();

builder.Services.AddScoped<ApiCaller>();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.VisibleStateDuration = 6000;
});

await builder.Build().RunAsync();
