using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.Extensions.WebEncoders;
using EmirCoaching.Web.Infrastructure;
using EmirCoaching.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Türkçe karakterler HTML'de &#x15F; gibi kodlanmasın (okunur kaynak, daha küçük sayfa)
builder.Services.Configure<WebEncoderOptions>(o => o.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton(TimeProvider.System);

// Sayfa verisi API'den sunucu tarafında alınır; istemciler panelle aynı OpenAPI belgesinden üretilir
var apiBaseUrl = new Uri(builder.Configuration["Api:BaseUrl"]
    ?? throw new InvalidOperationException("Api:BaseUrl ayarı eksik."));
builder.Services.AddApiClients(apiBaseUrl);
builder.Services.AddScoped<SiteApi>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/hata/500");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 404 ve diğer hata kodları tasarımdaki hata sayfalarıyla gösterilir
app.UseStatusCodePagesWithReExecute("/hata/{0}");

// Eski statik adresler (paketler.html, paket-detay.html?paket=...) yeni adreslere kalıcı yönlenir
app.UseMiddleware<LegacyHtmlRedirectMiddleware>();

app.UseStaticFiles();
app.MapControllers();

app.Run();
