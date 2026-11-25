using Microsoft.AspNetCore.Http.Extensions;

namespace EmirCoaching.Web.Infrastructure;

/// <summary>
/// Statik sitenin .html adreslerini (paylaşılmış linkler, arama motoru kayıtları) yeni adreslere 301 ile yönlendirir.
/// Eski paket kodları (temel / orta / premium + yer) paket slug'ına çevrilir.
/// </summary>
public sealed class LegacyHtmlRedirectMiddleware(RequestDelegate next)
{
    private static readonly Dictionary<string, string> PackageSlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        ["temel"] = "baslangic-deneme",
        ["orta"] = "donusum-protokolu",
        ["premium"] = "vip-yasam-tarzi"
    };

    private static readonly Dictionary<string, string> SimplePages = new(StringComparer.OrdinalIgnoreCase)
    {
        ["/index.html"] = "/",
        ["/paketler.html"] = "/paketler",
        ["/blog.html"] = "/blog",
        ["/blog-detay.html"] = "/blog",
        ["/degisimler.html"] = "/degisimler",
        ["/hakkimda.html"] = "/hakkimda",
        ["/iletisim.html"] = "/iletisim",
        ["/diyet-plani-detay.html"] = "/diyet-plani",
        ["/bulten-iptal.html"] = "/bulten-iptal"
    };

    public Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (!path.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
            return next(context);

        var target = Resolve(path, context.Request.Query);
        if (target is null)
            return next(context);

        context.Response.Redirect(target, permanent: true);
        return Task.CompletedTask;
    }

    private static string? Resolve(string path, IQueryCollection query)
    {
        switch (path.ToLowerInvariant())
        {
            case "/paket-detay.html":
                return PackageSlug(query) is { } slug ? $"/paketler/{slug}" : "/paketler";
            case "/diyet-formu.html":
                return PackageSlug(query) is { } formSlug ? $"/diyet-formu?paket={formSlug}" : "/diyet-formu";
            case "/gizlilik-politikasi.html":
                return query["tur"].FirstOrDefault() is { Length: > 0 } tur
                    ? $"/yasal/{Uri.EscapeDataString(tur)}"
                    : "/gizlilik-politikasi";
        }

        if (!SimplePages.TryGetValue(path, out var page))
            return null;

        // Filtre gibi diğer parametreler (?kategori=evde) korunur
        return query.Count == 0 ? page : page + new QueryBuilder(query.SelectMany(q => q.Value.Select(v =>
            new KeyValuePair<string, string>(q.Key, v ?? string.Empty)))).ToQueryString();
    }

    private static string? PackageSlug(IQueryCollection query)
    {
        if (!PackageSlugs.TryGetValue(query["paket"].FirstOrDefault() ?? string.Empty, out var slug))
            return null;
        return query["yer"].FirstOrDefault() == "evde" ? $"{slug}-evde" : slug;
    }
}
