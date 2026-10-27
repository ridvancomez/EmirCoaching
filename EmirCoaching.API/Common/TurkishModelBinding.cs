using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using EmirCoaching.Business.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace EmirCoaching.API.Common;

// Servise ulaşmadan dönen ASP.NET mesajları (bozuk JSON, yanlış tip, eksik alan, 415 gibi durumlar) Türkçe.
// Alan adları FieldNames'ten gelir; kullanıcının girdiği değer mesaja yazılmaz.
public static partial class TurkishModelBinding
{
    private const string UnreadableBody = "Gönderilen veri okunamadı.";

    public static void Configure(MvcOptions options)
    {
        var messages = options.ModelBindingMessageProvider;
        messages.SetMissingBindRequiredValueAccessor(field => $"'{FieldNames.Get(field)}' gönderilmedi.");
        messages.SetMissingKeyOrValueAccessor(() => "Bir değer girilmesi gerekiyor.");
        messages.SetMissingRequestBodyRequiredValueAccessor(() => "İstek gövdesi boş olamaz.");
        messages.SetValueMustNotBeNullAccessor(_ => "Bu alan boş olamaz.");
        messages.SetAttemptedValueIsInvalidAccessor((_, field) => $"'{FieldNames.Get(field)}' için girilen değer geçerli değil.");
        messages.SetNonPropertyAttemptedValueIsInvalidAccessor(_ => "Girilen değer geçerli değil.");
        messages.SetUnknownValueIsInvalidAccessor(field => $"'{FieldNames.Get(field)}' için girilen değer geçerli değil.");
        messages.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Girilen değer geçerli değil.");
        messages.SetValueIsInvalidAccessor(_ => "Girilen değer geçerli değil.");
        messages.SetValueMustBeANumberAccessor(field => $"'{FieldNames.Get(field)}' bir sayı olmalı.");
        messages.SetNonPropertyValueMustBeANumberAccessor(() => "Değer bir sayı olmalı.");

        options.ModelMetadataDetailsProviders.Add(new RequiredMessageProvider());
    }

    // [ApiController]'ın kendi ürettiği hata yanıtlarının başlıkları (ör. 415)
    public static void Configure(ApiBehaviorOptions options)
    {
        var titles = new Dictionary<int, string>
        {
            [StatusCodes.Status400BadRequest] = "Gönderilen bilgiler geçersiz.",
            [StatusCodes.Status401Unauthorized] = "Bu işlem için giriş yapmalısınız.",
            [StatusCodes.Status403Forbidden] = "Bu işlem için yetkiniz yok.",
            [StatusCodes.Status404NotFound] = "Kayıt bulunamadı.",
            [StatusCodes.Status405MethodNotAllowed] = "Bu istek türü desteklenmiyor.",
            [StatusCodes.Status406NotAcceptable] = "İstenen yanıt biçimi desteklenmiyor.",
            [StatusCodes.Status409Conflict] = "İşlem mevcut kayıtlarla çakışıyor.",
            [StatusCodes.Status415UnsupportedMediaType] = "Veri JSON olarak gönderilmeli.",
            [StatusCodes.Status422UnprocessableEntity] = "Gönderilen bilgiler işlenemedi.",
            [StatusCodes.Status500InternalServerError] = "Beklenmeyen bir hata oluştu."
        };

        foreach (var (status, title) in titles)
        {
            if (options.ClientErrorMapping.TryGetValue(status, out var mapping))
                mapping.Title = title;
            else
                options.ClientErrorMapping[status] = new ClientErrorData { Title = title };
        }
    }

    // ModelState → mesaj listesi. Bozuk JSON hatalarının mesajı boştur (ayrıntı sızdırılmaz), alan adından üretilir.
    public static List<string> GetErrors(ModelStateDictionary modelState)
    {
        var errors = modelState
            .SelectMany(entry => entry.Value!.Errors.Select(error =>
                string.IsNullOrEmpty(error.ErrorMessage) ? InvalidValue(entry.Key) : error.ErrorMessage))
            .Distinct()
            .ToList();

        // Daha belirli bir mesaj varsa genel "okunamadı" mesajı gösterilmez
        if (errors.Count > 1)
            errors.Remove(UnreadableBody);

        return errors;
    }

    // "$.highlights[0].text" → "'Metin' alanının değeri geçersiz."; "$" (JSON hiç okunamadı) → genel mesaj
    private static string InvalidValue(string key)
    {
        var field = IndexPattern().Replace(key.Split('.')[^1], string.Empty).TrimStart('$');
        return field.Length == 0 ? UnreadableBody : $"'{FieldNames.Get(field)}' alanının değeri geçersiz.";
    }

    [GeneratedRegex(@"\[\d+\]")]
    private static partial Regex IndexPattern();

    // Nullable olmayan alanlara ASP.NET'in eklediği örtük [Required] mesajı ("The X field is required.")
    private sealed class RequiredMessageProvider : IValidationMetadataProvider
    {
        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (var required in context.ValidationMetadata.ValidatorMetadata.OfType<RequiredAttribute>())
            {
                if (required.ErrorMessage is not null)
                    continue;

                // Parametrenin kendisi (ör. request) yalnızca gövde okunamadığında boş kalır
                required.ErrorMessage = context.Key.MetadataKind == ModelMetadataKind.Parameter
                    ? UnreadableBody
                    : $"'{FieldNames.Get(context.Key.Name ?? string.Empty)}' alanı zorunludur.";
            }
        }
    }
}
