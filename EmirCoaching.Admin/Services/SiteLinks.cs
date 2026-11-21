namespace EmirCoaching.Admin.Services;

// Panelden sitedeki sayfaya "Sitede gör" bağlantıları. Adres wwwroot/appsettings(.Development).json → SiteBaseUrl
public static class SiteLinks
{
    public static string SiteBaseUrl { get; set; } = "/";

    // Sitenin kendi görselleri göreli gelir (/images/...); panel başka adreste çalıştığı için sitenin adresiyle gösterilir.
    // Yüklenen görseller (/uploads/...) API'den zaten tam URL ile gelir. Kaydedilen değer değişmez.
    public static string? Image(string? url) =>
        url is ['/', not '/', ..] ? SiteBaseUrl.TrimEnd('/') + url : url;
}
