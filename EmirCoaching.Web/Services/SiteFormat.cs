using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using EmirCoaching.Web.Api;
using Microsoft.AspNetCore.Html;

namespace EmirCoaching.Web.Services;

/// <summary>View'larda tekrar eden biçimlendirme: fiyat, vurgulu başlık, tarih, ikon anahtarı → iconify adı.</summary>
public static class SiteFormat
{
    private static readonly CultureInfo Turkish = CultureInfo.GetCultureInfo("tr-TR");
    private static readonly HtmlEncoder Encoder = HtmlEncoder.Create(UnicodeRanges.All);

    /// <summary>₺8.999</summary>
    public static string Price(double value) => "₺" + value.ToString("N0", Turkish);

    /// <summary>"12 Mart 2026"</summary>
    public static string Date(DateTime value) => value.ToString("d MMMM yyyy", Turkish);

    /// <summary>"01 Ocak 2024"</summary>
    public static string LongDate(DateTime value) => value.ToString("dd MMMM yyyy", Turkish);

    /// <summary>"24 Eki 2024"</summary>
    public static string ShortDate(DateTime value) => value.ToString("d MMM yyyy", Turkish);

    /// <summary>"[15.10.2024]" biçimi için "15.10.2024"</summary>
    public static string NumericDate(DateTime value) => value.ToString("dd.MM.yyyy", Turkish);

    /// <summary>"İbrahim Emir" → "İE"</summary>
    public static string Initials(string name) =>
        string.Concat(name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Take(2).Select(w => char.ToUpper(w[0], Turkish)));

    /// <summary>Panelde satır satır yazılan metni paragraflara böler (boş satırlar atlanır).</summary>
    public static IEnumerable<string> Paragraphs(string? text) =>
        (text ?? string.Empty).Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    /// <summary>"01", "02" — adım numarası</summary>
    public static string Number(int value) => value.ToString("N0", Turkish);

    public static string StepNumber(int index) => (index + 1).ToString("00");

    /// <summary>
    /// Başlıktaki vurgulu kelimeyi span'a sarar (ör. "daha GÜÇLÜ hale" → GÜÇLÜ turuncu); metin HTML olarak kaçırılır.
    /// Panelde başlığa yazılan satır sonu &lt;br&gt; olur.
    /// </summary>
    public static IHtmlContent Highlight(string? text, string? accent, string accentClass)
    {
        text ??= string.Empty;
        var index = string.IsNullOrEmpty(accent) ? -1 : text.IndexOf(accent, StringComparison.Ordinal);
        if (index < 0)
            return new HtmlString(Lines(text));

        return new HtmlString(Lines(text[..index])
            + $"<span class=\"{accentClass}\">{Encode(accent!)}</span>"
            + Lines(text[(index + accent!.Length)..]));
    }

    private static string Lines(string text) =>
        string.Join("<br>", text.Replace("\r", string.Empty).Split('\n').Select(Encode));

    private static string Encode(string text) => Encoder.Encode(text);

    /// <summary>Paket kartı ve öne çıkan maddelerin ikonları (panelde IconKeys ile aynı anahtarlar).</summary>
    public static string HighlightIcon(string key) => key switch
    {
        "antrenman" => "solar:cup-broken",
        "beslenme" => "fluent:food-apple-24-regular",
        "tahlil" => "solar:test-tube-broken",
        "takvim" => "solar:calendar-broken",
        "whatsapp" => "ic:outline-whatsapp",
        "zoom" => "mdi:video-outline",
        _ => "mdi:check"
    };

    /// <summary>Paket detayındaki modül kartlarının ikonları.</summary>
    public static string ModuleIcon(string key) => key switch
    {
        "antrenman" => "mdi:dumbbell",
        "beslenme" => "mdi:silverware-fork-knife",
        "takip-olcum" => "mdi:chart-line",
        "kocluk" => "mdi:whistle",
        _ => "mdi:check-circle"
    };

    /// <summary>Ana sayfa "Satın aldıktan sonra ne oluyor?" adımlarının ikonları.</summary>
    public static string PurchaseStepIcon(string key) => key switch
    {
        "form" => "solar:clipboard-list-broken",
        "saat" => "solar:clock-circle-broken",
        "takvim" => "solar:calendar-broken",
        "revize" => "solar:refresh-circle-broken",
        _ => "solar:check-circle-broken"
    };

    public static string LegalSlug(LegalDocumentType type) => type switch
    {
        LegalDocumentType.Kvkk => "kvkk",
        LegalDocumentType.AcikRiza => "acik-riza",
        LegalDocumentType.Cerez => "cerez",
        LegalDocumentType.IptalIade => "iptal-iade",
        LegalDocumentType.TicariIleti => "ticari-ileti",
        LegalDocumentType.MesafeliSatis => "mesafeli-satis",
        _ => "gizlilik"
    };

    public static LegalDocumentType? LegalType(string? slug) =>
        Enum.GetValues<LegalDocumentType>().Cast<LegalDocumentType?>().FirstOrDefault(t => LegalSlug(t!.Value) == slug);
}

/// <summary>
/// Pakete özel maddeler: aynı yerdeki (salon/ev) paketlerin bir kısmında kilitli olan madde, açık olduğu pakette
/// "Pakete Özel", kilitli olduğu pakette "6 ve 12 Aylık Pakette" etiketini alır.
/// </summary>
public sealed class PackagePerks(IEnumerable<PackageResponse> packages, PackageLocation location)
{
    private readonly List<PackageResponse> _packages = packages.Where(p => p.Location == location).ToList();

    public string? Label(string text, bool isLocked)
    {
        var unlockedIn = _packages
            .Where(p => p.Highlights.Any(h => h.Text == text && !h.IsLocked))
            .Select(p => p.DurationMonths)
            .ToList();

        if (isLocked)
            return unlockedIn.Count == 0 ? null : string.Join(" ve ", unlockedIn) + " Aylık Pakette";

        return _packages.Any(p => p.Highlights.Any(h => h.Text == text && h.IsLocked)) ? "Pakete Özel" : null;
    }
}
