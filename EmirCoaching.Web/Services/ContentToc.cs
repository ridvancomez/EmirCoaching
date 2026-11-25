using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Html.Parser;

namespace EmirCoaching.Web.Services;

public sealed record TocEntry(string Id, string Text);

/// <summary>Panelde editörle girilen HTML ve ondan çıkarılan içindekiler.</summary>
public sealed record TocContent(string Html, IReadOnlyList<TocEntry> Entries);

/// <summary>
/// İçindekiler listesini makalenin h2 başlıklarından kurar. Kimliği olmayan başlığa Türkçe karakterleri
/// sadeleştirilmiş bir kimlik verilir; editör çıktısı bu yüzden elle kimlik gerektirmez.
/// Başlığı olmayan, aria-label'lı ilk bölüm (ör. "Giriş") de listeye girer.
/// </summary>
public static partial class ContentToc
{
    private static readonly HtmlParser Parser = new();

    /// <param name="headingClass">Kimliksiz başlıklara eklenecek sınıf (yasal metinde stil için); yoksa null</param>
    public static TocContent Build(string? html, string? headingClass = null)
    {
        if (string.IsNullOrWhiteSpace(html))
            return new TocContent(string.Empty, []);

        var document = Parser.ParseDocument($"<body>{html}</body>");
        var body = document.Body!;
        var used = new HashSet<string>(body.QuerySelectorAll("[id]").Select(e => e.Id!));
        var entries = new List<TocEntry>();

        foreach (var element in body.QuerySelectorAll("section[aria-label][id], h2"))
        {
            if (element.LocalName == "section")
            {
                // Başlıksız giriş bölümü
                if (element.QuerySelector("h2") is null)
                    entries.Add(new TocEntry(element.Id!, element.GetAttribute("aria-label")!));
                continue;
            }

            if (string.IsNullOrEmpty(element.Id))
            {
                element.Id = Unique(Slug(element.TextContent), used);
                if (headingClass is not null && !element.ClassList.Contains("gizlilik-bolum__title"))
                    element.ClassList.Add(headingClass);
            }

            entries.Add(new TocEntry(element.Id!, LeadingNumber().Replace(Collapse(element.TextContent), string.Empty)));
        }

        return new TocContent(body.InnerHtml, entries);
    }

    /// <summary>"Bazal Metabolizma Hızı" → "bazal-metabolizma-hizi"</summary>
    public static string Slug(string text)
    {
        var lower = text.ToLower(CultureInfo.GetCultureInfo("tr-TR"))
            .Replace('ı', 'i').Replace('ğ', 'g').Replace('ü', 'u').Replace('ş', 's').Replace('ö', 'o').Replace('ç', 'c');
        var normalized = new StringBuilder();
        foreach (var c in lower.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                normalized.Append(c);
        }

        var slug = NonAlphanumeric().Replace(normalized.ToString(), "-").Trim('-');
        return slug.Length == 0 ? "bolum" : slug;
    }

    private static string Unique(string slug, HashSet<string> used)
    {
        var candidate = slug;
        for (var i = 2; !used.Add(candidate); i++)
            candidate = $"{slug}-{i}";
        return candidate;
    }

    private static string Collapse(string text) => Whitespace().Replace(text, " ").Trim();

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonAlphanumeric();

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();

    // "1. Genel Bilgilendirme" → numara listede ayrıca yazılır
    [GeneratedRegex(@"^\d+\.\s*")]
    private static partial Regex LeadingNumber();
}
