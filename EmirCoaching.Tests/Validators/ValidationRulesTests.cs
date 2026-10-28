using AwesomeAssertions;
using EmirCoaching.Business.Validation;
using FluentValidation;

namespace EmirCoaching.Tests.Validators;

// Ortak kuralları gerçek bir Request'e bağlamadan, yalnızca o kuralı taşıyan küçük validator'larla dener
public class ValidationRulesTests
{
    private record Model(string Text = "", decimal Number = 0, string? OptionalText = null);

    private static bool IsValid(Action<InlineValidator<Model>> rule, Model model)
    {
        var validator = new InlineValidator<Model>();
        rule(validator);
        return validator.Validate(model).IsValid;
    }

    [Theory]
    [InlineData("beslenme", true)]
    [InlineData("kas-gelisimi-101", true)]
    [InlineData("", false)]
    [InlineData("Beslenme", false)]        // büyük harf
    [InlineData("kas gelisimi", false)]    // boşluk
    [InlineData("-beslenme", false)]       // tireyle başlıyor
    [InlineData("kas--gelisimi", false)]   // çift tire
    [InlineData("gelişim", false)]         // Türkçe karakter
    public void Slug_ValidatesFormat(string slug, bool expected)
    {
        IsValid(v => v.RuleFor(x => x.Text).Slug(), new Model(slug)).Should().Be(expected);
    }

    [Fact]
    public void Slug_LongerThan120_IsInvalid()
    {
        IsValid(v => v.RuleFor(x => x.Text).Slug(), new Model(new string('a', 121))).Should().BeFalse();
    }

    [Theory]
    [InlineData("0532 123 45 67", true)]
    [InlineData("+90 (532) 123-45-67", true)]
    [InlineData("05321234567", true)]
    [InlineData("12345", false)]           // çok kısa
    [InlineData("0532 abc 45 67", false)]  // harf
    [InlineData("", false)]
    public void Phone_ValidatesFormat(string phone, bool expected)
    {
        IsValid(v => v.RuleFor(x => x.Text).Phone(), new Model(phone)).Should().Be(expected);
    }

    [Theory]
    [InlineData("https://emircoaching.com/paket", true)]
    [InlineData("http://emircoaching.com", false)]   // https değil
    [InlineData("emircoaching.com", false)]          // mutlak adres değil
    [InlineData("javascript:alert(1)", false)]
    public void HttpsUrl_RequiresAbsoluteHttpsAddress(string url, bool expected)
    {
        IsValid(v => v.RuleFor(x => x.Text).HttpsUrl(), new Model(url)).Should().Be(expected);
    }

    [Theory]
    [InlineData("https://www.youtube.com/watch?v=abc", true)]
    [InlineData("https://youtube.com/shorts/abc", true)]
    [InlineData("https://youtu.be/abc", true)]
    [InlineData("http://www.youtube.com/watch?v=abc", false)]  // https değil
    [InlineData("https://vimeo.com/123", false)]
    [InlineData("https://youtu.be.evil.com/abc", false)]
    [InlineData("", false)]
    public void YouTubeUrl_AcceptsOnlyYouTubeHosts(string url, bool expected)
    {
        IsValid(v => v.RuleFor(x => x.Text).YouTubeUrl(), new Model(url)).Should().Be(expected);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(4.5, true)]
    [InlineData(5, true)]
    [InlineData(4.3, false)]   // yarım puanlık adım değil
    [InlineData(-0.5, false)]
    [InlineData(5.5, false)]
    public void Rating_AllowsHalfStepsBetweenZeroAndFive(double rating, bool expected)
    {
        // InlineData decimal kabul etmediği için double gelir
        IsValid(v => v.RuleFor(x => x.Number).Rating(), new Model(Number: (decimal)rating)).Should().Be(expected);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("/uploads/blog/a.jpg", true)]
    [InlineData("/images/logo.png", true)]
    [InlineData("uploads/a.jpg", false)]               // '/' ile başlamıyor
    [InlineData("/uploads/../appsettings.json", false)] // üst klasöre çıkma
    [InlineData("https://baska.site/a.jpg", false)]
    public void SitePath_RequiresRootRelativePathWithoutTraversal(string? path, bool expected)
    {
        IsValid(v => v.RuleFor(x => x.OptionalText).SitePath(), new Model(OptionalText: path)).Should().Be(expected);
    }
}
