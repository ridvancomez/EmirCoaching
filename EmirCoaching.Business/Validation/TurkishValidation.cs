using System.Globalization;
using FluentValidation;
using FluentValidation.Resources;

namespace EmirCoaching.Business.Validation;

// FluentValidation'ın Türkçe ayarı: alan adları FieldNames'ten, hazır mesajlar aşağıdaki metinlerden.
// Kütüphanenin kendi Türkçe çevirisinde hatalar var (ör. e-posta mesajında çift boşluk), kullandığımız kurallar burada ezilir.
public static class TurkishValidation
{
    public static void Configure()
    {
        ValidatorOptions.Global.LanguageManager = new TurkishLanguageManager { Culture = new CultureInfo("tr") };
        ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => member is null ? null : FieldNames.Find(member.Name);

        // Alan başına tek mesaj: ilk kural geçmezse o alanın diğer kuralları çalışmaz ("boş olamaz" + "geçersiz" birlikte gelmez)
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
    }

    private sealed class TurkishLanguageManager : LanguageManager
    {
        public TurkishLanguageManager()
        {
            Add("NotEmptyValidator", "'{PropertyName}' boş olamaz.");
            Add("NotNullValidator", "'{PropertyName}' boş olamaz.");
            Add("EmptyValidator", "'{PropertyName}' boş olmalı.");
            Add("NullValidator", "'{PropertyName}' boş olmalı.");
            Add("MaximumLengthValidator", "'{PropertyName}' en fazla {MaxLength} karakter olabilir. {TotalLength} karakter girdiniz.");
            Add("MinimumLengthValidator", "'{PropertyName}' en az {MinLength} karakter olmalı. {TotalLength} karakter girdiniz.");
            Add("LengthValidator", "'{PropertyName}' {MinLength} ile {MaxLength} karakter arasında olmalı. {TotalLength} karakter girdiniz.");
            Add("ExactLengthValidator", "'{PropertyName}' tam {MaxLength} karakter olmalı. {TotalLength} karakter girdiniz.");
            Add("EmailValidator", "'{PropertyName}' geçerli bir e-posta adresi değil.");
            Add("InclusiveBetweenValidator", "'{PropertyName}' {From} ile {To} arasında olmalı.");
            Add("ExclusiveBetweenValidator", "'{PropertyName}' {From} ile {To} arasında olmalı (sınırlar hariç).");
            Add("GreaterThanValidator", "'{PropertyName}' {ComparisonValue} değerinden büyük olmalı.");
            Add("GreaterThanOrEqualValidator", "'{PropertyName}' en az {ComparisonValue} olmalı.");
            Add("LessThanValidator", "'{PropertyName}' {ComparisonValue} değerinden küçük olmalı.");
            Add("LessThanOrEqualValidator", "'{PropertyName}' en fazla {ComparisonValue} olabilir.");
            Add("EqualValidator", "'{PropertyName}' '{ComparisonValue}' değerine eşit olmalı.");
            Add("NotEqualValidator", "'{PropertyName}' '{ComparisonValue}' değerine eşit olmamalı.");
            Add("EnumValidator", "'{PropertyName}' için geçersiz bir seçim yapıldı.");
            Add("PredicateValidator", "'{PropertyName}' geçerli değil.");
            Add("AsyncPredicateValidator", "'{PropertyName}' geçerli değil.");
            Add("RegularExpressionValidator", "'{PropertyName}' doğru biçimde değil.");
            Add("ScalePrecisionValidator",
                "'{PropertyName}' en fazla {ExpectedPrecision} basamak ({ExpectedScale} ondalık) olabilir. {Digits} basamak ve {ActualScale} ondalık girdiniz.");
        }

        private void Add(string key, string message) => AddTranslation("tr", key, message);
    }
}
