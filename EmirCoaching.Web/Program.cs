using EmirCoaching.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

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
