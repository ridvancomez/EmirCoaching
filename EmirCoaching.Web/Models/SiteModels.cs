using EmirCoaching.Web.Api;
using EmirCoaching.Web.Services;

namespace EmirCoaching.Web.Models;

/// <summary>Paket kartı (ana sayfa ve paketler sayfası aynı tasarımı kullanır).</summary>
/// <param name="Rank">Yerdeki sırası (1, 2, 3): kartın rütbe ikonu ve rengi buna göre</param>
/// <param name="ElementId">Menüdeki /paketler#slug bağlantıları için; ana sayfada yok</param>
public sealed record PackageCardModel(
    PackageResponse Package,
    int Rank,
    PackagePerks Perks,
    string? ElementId,
    string NameUrl,
    string WhatsAppUrl)
{
    public string TitleId => ElementId is null ? $"pkg-{Rank}-title" : $"{ElementId}-title";
    public string LocationNote => Package.Location == PackageLocation.Evde ? " (evde)" : string.Empty;
}

/// <summary>Paketler sayfasındaki bir yer grubu (Salonda / Evde).</summary>
public sealed record PackageGroupModel(string Key, string Title, string Label, IReadOnlyList<PackageResponse> Packages);

public sealed class HomeViewModel
{
    public required LayoutData Layout { get; init; }
    public PageContentResponse? Hero { get; init; }
    public required IReadOnlyList<PurchaseStepResponse> PurchaseSteps { get; init; }
    public CoachProfileResponse? Coach { get; init; }
    public required IReadOnlyList<CertificateResponse> Certificates { get; init; }
    public required IReadOnlyList<TransformationResponse> Transformations { get; init; }
    public required IReadOnlyList<TestimonialResponse> Testimonials { get; init; }
    public required IReadOnlyList<FaqResponse> Faqs { get; init; }
    public required IReadOnlyList<SocialPostResponse> YouTubePosts { get; init; }
    public required IReadOnlyList<SocialPostResponse> InstagramPosts { get; init; }
}

public sealed class PackagesViewModel
{
    public required LayoutData Layout { get; init; }
    public PageContentResponse? Hero { get; init; }
    public required IReadOnlyList<PackageGroupModel> Groups { get; init; }
}

public sealed class PackageDetailViewModel
{
    public required LayoutData Layout { get; init; }
    public required PackageDetailResponse Package { get; init; }
    public required PackagePerks Perks { get; init; }
    /// <summary>Panelde "paket-detay" sayfa içeriği (hero başlığı ve görseli)</summary>
    public PageContentResponse? Hero { get; init; }

    /// <summary>Evde ve salonda paketleri aynı adı taşır; detayda hangisi olduğu adda belirtilir</summary>
    public bool IsHome => Package.Location == PackageLocation.Evde;
    public string DisplayName => IsHome ? $"{Package.Name} (Evde)" : Package.Name;
}

public sealed class BlogListViewModel
{
    public PageContentResponse? Hero { get; init; }
    public required IReadOnlyList<BlogCategoryResponse> Categories { get; init; }
    public BlogPostResponse? Featured { get; init; }
    /// <summary>"Tüm Yazılar": öne çıkan hariç; kategori filtresi tarayıcıda (js/blog.js)</summary>
    public required IReadOnlyList<BlogPostResponse> Posts { get; init; }
    /// <summary>Liste yanıtında yazar yok; yazıları koç yazar</summary>
    public string? AuthorName { get; init; }
}

public sealed class BlogDetailViewModel
{
    public required BlogPostDetailResponse Post { get; init; }
    public required TocContent Content { get; init; }
    public string? HeroImageUrl { get; init; }
}

public sealed class TransformationsViewModel
{
    public PageContentResponse? Hero { get; init; }
    public required IReadOnlyList<TransformationDetailResponse> Carousel { get; init; }
    public required IReadOnlyList<TransformationDetailResponse> Gallery { get; init; }
}

public sealed class AboutViewModel
{
    public PageContentResponse? Hero { get; init; }
    public required IReadOnlyList<SiteStatResponse> Stats { get; init; }
    public CoachProfileResponse? Coach { get; init; }
    public required IReadOnlyList<CertificateResponse> Certificates { get; init; }
    /// <summary>"Sayılar Yalan Söylemez": ana sayfada gösterilen değişimlerin ilk ikisi</summary>
    public required IReadOnlyList<TransformationResponse> Transformations { get; init; }
}

public sealed class ContactPageViewModel
{
    public required LayoutData Layout { get; init; }
    public PageContentResponse? Hero { get; init; }
}

public sealed class DietPlanViewModel
{
    public required LayoutData Layout { get; init; }
    public PageContentResponse? Hero { get; init; }
    public DietProgramResponse? Program { get; init; }
    public required IReadOnlyList<DietProgramStepResponse> Steps { get; init; }
}

public sealed class DietFormViewModel
{
    public required LayoutData Layout { get; init; }
    public PageContentResponse? Hero { get; init; }
    /// <summary>Paket detayındaki "Satın Al"dan gelen ?paket=slug; formda seçili gelir</summary>
    public string? SelectedSlug { get; init; }
}

public sealed class LegalViewModel
{
    public required string Slug { get; init; }
    public required bool IsPrivacyPolicy { get; init; }
    /// <summary>Henüz yayınlanmamış metinde null</summary>
    public LegalDocumentResponse? Document { get; init; }
    public required IReadOnlyList<LegalDocumentRevisionResponse> Revisions { get; init; }
    public required TocContent Content { get; init; }
    /// <summary>Hero'daki giriş metni; yalnızca Gizlilik Politikası'nda</summary>
    public string? Lead { get; init; }
    public required string LegalEmail { get; init; }
    public required DateTime Now { get; init; }

    public string Title => Document?.Title ?? "Belge henüz yayınlanmadı";
    public string Url => IsPrivacyPolicy ? "/gizlilik-politikasi" : $"/yasal/{Slug}";
    /// <summary>Yürürlük: bu sürümün yayın tarihi; son güncelleme: en yeni revizyon</summary>
    public DateTime EffectiveAt => Document?.PublishedAt ?? Now;
    public DateTime UpdatedAt => Revisions.Count > 0 ? Revisions[0].RevisedAt : EffectiveAt;
}
