using EmirCoaching.Web.Api;
using EmirCoaching.Web.Models;
using EmirCoaching.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.Web.Controllers;

// Sayfa verisi API'den SiteApi ile alınır; controller yalnızca view modelini toplar
public class HomeController(SiteApi api) : Controller
{
    private const int YouTubeCount = 1;
    private const int InstagramCount = 3;

    [HttpGet("/")]
    public async Task<IActionResult> Index()
    {
        var layout = api.GetLayoutAsync();
        var hero = api.GetPageContentAsync("home");
        var steps = api.GetPurchaseStepsAsync();
        var coach = api.GetCoachProfileAsync();
        var certificates = api.GetCertificatesAsync();
        var transformations = api.GetHomeTransformationsAsync();
        var testimonials = api.GetHomeTestimonialsAsync();
        var faqs = api.GetFaqsAsync(FaqGroup.Genel);
        var youTube = api.GetSocialPostsAsync(SocialPlatform.YouTube, YouTubeCount);
        var instagram = api.GetSocialPostsAsync(SocialPlatform.Instagram, InstagramCount);

        return View(new HomeViewModel
        {
            Layout = await layout,
            Hero = await hero,
            PurchaseSteps = await steps,
            Coach = await coach,
            Certificates = await certificates,
            Transformations = await transformations,
            Testimonials = await testimonials,
            Faqs = await faqs,
            YouTubePosts = await youTube,
            InstagramPosts = await instagram
        });
    }
}

public class PaketlerController(SiteApi api) : Controller
{
    [HttpGet("/paketler")]
    public async Task<IActionResult> Index()
    {
        var layout = await api.GetLayoutAsync();
        var hero = await api.GetPageContentAsync("paketler");

        return View(new PackagesViewModel
        {
            Layout = layout,
            Hero = hero,
            Groups = new List<PackageGroupModel>
            {
                new("salonda", "Salonda Paketler", "salon", layout.Packages.Where(p => p.Location == PackageLocation.Salonda).ToList()),
                new("evde", "Evde Paketler", "evde", layout.Packages.Where(p => p.Location == PackageLocation.Evde).ToList())
            }.Where(g => g.Packages.Count > 0).ToList()
        });
    }

    [HttpGet("/paketler/{slug}")]
    public async Task<IActionResult> Detay(string slug)
    {
        var package = await api.GetPackageAsync(slug);
        if (package is null)
            return NotFound();

        var layout = await api.GetLayoutAsync();
        return View(new PackageDetailViewModel
        {
            Layout = layout,
            Package = package,
            Perks = new PackagePerks(layout.Packages, package.Location),
            Hero = await api.GetPageContentAsync("paket-detay")
        });
    }
}

public class BlogController(SiteApi api) : Controller
{
    // Liste tek sayfada; kategori filtresi tarayıcıda yapılır (API'nin sayfa sınırı)
    private const int ListSize = 100;

    [HttpGet("/blog")]
    public async Task<IActionResult> Index()
    {
        var hero = api.GetPageContentAsync("blog");
        var categories = api.GetBlogCategoriesAsync();
        var featured = api.GetFeaturedPostAsync();
        var posts = api.GetBlogPostsAsync(1, ListSize);
        var coach = api.GetCoachProfileAsync();
        var featuredPost = await featured;

        return View(new BlogListViewModel
        {
            Hero = await hero,
            Categories = await categories,
            Featured = featuredPost,
            Posts = (await posts).Items.Where(p => p.Id != featuredPost?.Id).ToList(),
            AuthorName = (await coach)?.FullName
        });
    }

    [HttpGet("/blog/{slug}")]
    public async Task<IActionResult> Detay(string slug)
    {
        var post = await api.GetBlogPostAsync(slug);
        if (post is null)
            return NotFound();

        var hero = await api.GetPageContentAsync("blog");
        return View(new BlogDetailViewModel
        {
            Post = post,
            Content = ContentToc.Build(post.ContentHtml),
            HeroImageUrl = hero?.BackgroundImageUrl
        });
    }
}

public class DegisimlerController(SiteApi api) : Controller
{
    [HttpGet("/degisimler")]
    public async Task<IActionResult> Index()
    {
        var hero = api.GetPageContentAsync("degisimler");
        var all = await api.GetTransformationDetailsAsync();

        return View(new TransformationsViewModel
        {
            Hero = await hero,
            Carousel = all.Where(t => t.Section == TransformationSection.Carousel).ToList(),
            Gallery = all.Where(t => t.Section == TransformationSection.Galeri).ToList()
        });
    }
}

public class SayfalarController(SiteApi api) : Controller
{
    private const int AboutTransformationCount = 2;

    [HttpGet("/hakkimda")]
    public async Task<IActionResult> Hakkimda()
    {
        var hero = api.GetPageContentAsync("hakkimda");
        var stats = api.GetSiteStatsAsync();
        var coach = api.GetCoachProfileAsync();
        var certificates = api.GetCertificatesAsync();
        var transformations = api.GetHomeTransformationsAsync();

        return View(new AboutViewModel
        {
            Hero = await hero,
            Stats = await stats,
            Coach = await coach,
            Certificates = await certificates,
            Transformations = (await transformations).Take(AboutTransformationCount).ToList()
        });
    }

    [HttpGet("/iletisim")]
    public async Task<IActionResult> Iletisim()
    {
        var layout = api.GetLayoutAsync();
        var hero = api.GetPageContentAsync("iletisim");
        return View(new ContactPageViewModel { Layout = await layout, Hero = await hero });
    }
}

public class DiyetController(SiteApi api) : Controller
{
    [HttpGet("/diyet-formu")]
    public async Task<IActionResult> Form([FromQuery] string? paket)
    {
        var layout = await api.GetLayoutAsync();
        var hero = await api.GetPageContentAsync("diyet-formu");
        return View(new DietFormViewModel
        {
            Layout = layout,
            Hero = hero,
            SelectedSlug = layout.Packages.Any(p => p.Slug == paket) ? paket : null
        });
    }

    [HttpGet("/diyet-plani")]
    public async Task<IActionResult> Plan()
    {
        var layout = api.GetLayoutAsync();
        var hero = api.GetPageContentAsync("diyet-plani-detay");
        var program = api.GetDietProgramAsync();
        var steps = api.GetDietProgramStepsAsync();

        return View(new DietPlanViewModel
        {
            Layout = await layout,
            Hero = await hero,
            Program = await program,
            Steps = await steps
        });
    }
}

public class YasalController(SiteApi api, TimeProvider clock) : Controller
{
    [HttpGet("/gizlilik-politikasi")]
    [HttpGet("/yasal/{tur}")]
    public async Task<IActionResult> Index(string? tur)
    {
        var type = tur is null ? LegalDocumentType.Gizlilik : SiteFormat.LegalType(tur);
        if (type is null)
            return NotFound();

        var layout = api.GetLayoutAsync();
        var hero = api.GetPageContentAsync("gizlilik-politikasi");
        var current = await api.GetLegalDocumentAsync(type.Value);

        return View(new LegalViewModel
        {
            Slug = SiteFormat.LegalSlug(type.Value),
            IsPrivacyPolicy = type == LegalDocumentType.Gizlilik,
            Document = current,
            Revisions = current is null ? [] : await api.GetLegalRevisionsAsync(type.Value),
            Content = ContentToc.Build(current?.ContentHtml, "yasal-icerik__baslik"),
            Lead = (await hero)?.Description,
            LegalEmail = (await layout).Settings.LegalEmail,
            Now = clock.GetLocalNow().DateTime
        });
    }
}

public class BultenController : Controller
{
    [HttpGet("/bulten-iptal")]
    public IActionResult Iptal() => View();
}

public class HataController : Controller
{
    // Durum kodu sayfaları (UseStatusCodePagesWithReExecute). 5xx sunucu hatası, diğer 4xx "sayfa yok" tasarımını
    // kullanır; aralık dışı kod (ör. /hata/200) 404 sayılır ki hata sayfası 200 ile dizine girmesin.
    [Route("/hata/{code:int}")]
    public IActionResult Index(int code)
    {
        if (code is < 400 or > 599)
            code = StatusCodes.Status404NotFound;

        Response.StatusCode = code;
        return code >= StatusCodes.Status500InternalServerError ? View("ServerError") : View("NotFound");
    }
}
