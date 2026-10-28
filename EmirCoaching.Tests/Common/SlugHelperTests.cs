using AwesomeAssertions;
using EmirCoaching.Business.Common;

namespace EmirCoaching.Tests.Common;

public class SlugHelperTests
{
    [Theory]
    [InlineData("Kilo Verme", "kilo-verme")]
    [InlineData("Kalori Açığı", "kalori-acigi")]
    [InlineData("IŞIK ÖĞÜN Çİğ Şüphe", "isik-ogun-cig-suphe")]   // Türkçe büyük harfler (I → ı → i, İ → i)
    [InlineData("#Beslenme", "beslenme")]
    [InlineData("  Kas   Gelişimi!  ", "kas-gelisimi")]
    [InlineData("5x5 Program", "5x5-program")]
    [InlineData("!!!", "")]
    public void ToSlug_ConvertsTurkishTextToSlug(string text, string expected)
    {
        SlugHelper.ToSlug(text).Should().Be(expected);
    }
}
