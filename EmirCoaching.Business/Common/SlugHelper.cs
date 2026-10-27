using System.Text;
using System.Text.RegularExpressions;

namespace EmirCoaching.Business.Common;

// "Kilo Verme" -> "kilo-verme", "Kalori Açığı" -> "kalori-acigi"
public static partial class SlugHelper
{
    private static readonly Dictionary<char, char> TurkishMap = new()
    {
        ['ç'] = 'c', ['ğ'] = 'g', ['ı'] = 'i', ['ö'] = 'o', ['ş'] = 's', ['ü'] = 'u'
    };

    public static string ToSlug(string text)
    {
        var builder = new StringBuilder(text.Length);
        foreach (var ch in text.Trim().ToLower(new System.Globalization.CultureInfo("tr-TR")))
            builder.Append(TurkishMap.GetValueOrDefault(ch, ch));

        return InvalidChars().Replace(builder.ToString(), "-").Trim('-');
    }

    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex InvalidChars();
}
