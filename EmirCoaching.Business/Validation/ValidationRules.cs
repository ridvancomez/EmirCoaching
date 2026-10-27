using FluentValidation;

namespace EmirCoaching.Business.Validation;

// Birden çok validator'da kullanılan ortak kurallar
public static class ValidationRules
{
    private const string SlugPattern = "^[a-z0-9]+(?:-[a-z0-9]+)*$";
    private const string PhonePattern = @"^\+?[0-9 ()-]{10,20}$";

    public static IRuleBuilderOptions<T, string> Slug<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().MaximumLength(120)
            .Matches(SlugPattern).WithMessage("Slug yalnızca küçük harf, rakam ve tire (-) içerebilir.");

    public static IRuleBuilderOptions<T, string> Phone<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().Matches(PhonePattern).WithMessage("Geçerli bir telefon numarası girin.");

    public static IRuleBuilderOptions<T, string> HttpsUrl<T>(this IRuleBuilder<T, string> rule) =>
        rule.MaximumLength(300)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps)
            .WithMessage("Geçerli bir https adresi girin.");

    public static IRuleBuilderOptions<T, string> YouTubeUrl<T>(this IRuleBuilder<T, string> rule) =>
        rule.NotEmpty().HttpsUrl()
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
                && (uri.Host.EndsWith("youtube.com", StringComparison.OrdinalIgnoreCase) || uri.Host.Equals("youtu.be", StringComparison.OrdinalIgnoreCase)))
            .WithMessage("Geçerli bir YouTube adresi girin.");

    // 0–5 arası, 0.5'lik adımlarla (veritabanındaki CK_Testimonials_Rating ile aynı)
    public static IRuleBuilderOptions<T, decimal> Rating<T>(this IRuleBuilder<T, decimal> rule) =>
        rule.InclusiveBetween(0, 5).Must(r => r * 2 == decimal.Truncate(r * 2))
            .WithMessage("Puan 0 ile 5 arasında, yarım puanlık adımlarla olmalı.");

    public static IRuleBuilderOptions<T, string?> SitePath<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(300)
            .Must(p => p is null || (p.StartsWith('/') && !p.Contains("..")))
            .WithMessage("Dosya yolu '/' ile başlamalı.");
}
