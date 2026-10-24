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

    public static IRuleBuilderOptions<T, string?> SitePath<T>(this IRuleBuilder<T, string?> rule) =>
        rule.MaximumLength(300)
            .Must(p => p is null || (p.StartsWith('/') && !p.Contains("..")))
            .WithMessage("Dosya yolu '/' ile başlamalı.");
}
