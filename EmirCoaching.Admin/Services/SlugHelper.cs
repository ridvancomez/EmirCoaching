using System.Text;
using System.Text.RegularExpressions;

namespace EmirCoaching.Admin.Services;

// "Beslenme & Sağlık" → "beslenme-saglik". API de aynı biçimi (küçük harf, rakam, tire) bekler.
public static partial class SlugHelper
{
    public static string From(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;

        var builder = new StringBuilder();
        foreach (var c in text.Trim().ToLower(new System.Globalization.CultureInfo("tr-TR")))
        {
            builder.Append(c switch
            {
                'ç' => 'c', 'ğ' => 'g', 'ı' => 'i', 'ö' => 'o', 'ş' => 's', 'ü' => 'u', 'â' => 'a', 'î' => 'i', 'û' => 'u',
                _ => c
            });
        }

        return Separators().Replace(builder.ToString(), "-").Trim('-');
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex Separators();
}
