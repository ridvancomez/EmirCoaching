using System.Text.Json;
using EmirCoaching.Admin.Api;

namespace EmirCoaching.Admin.Services;

// API hataları ProblemDetails'tir ({ title, errors[] }); kullanıcıya Türkçe mesajları gösterilir
public static class ApiErrors
{
    public static string Describe(ApiException ex) => Describe(ex.StatusCode, ex.Response);

    public static string Describe(int statusCode, string? json)
    {
        if (statusCode == 0)
            return "API'ye ulaşılamadı. Bağlantıyı kontrol edin.";

        try
        {
            using var doc = JsonDocument.Parse(json ?? "");
            var root = doc.RootElement;
            var errors = root.TryGetProperty("errors", out var list) && list.ValueKind == JsonValueKind.Array
                ? list.EnumerateArray().Select(e => e.GetString()).OfType<string>().ToList()
                : [];
            var title = root.TryGetProperty("title", out var t) ? t.GetString() : null;

            // 400'de başlık sabit ("Gönderilen bilgiler geçersiz."), asıl bilgi hata listesinde
            if (errors.Count > 0)
                return string.Join("\n", errors.Distinct());
            if (!string.IsNullOrWhiteSpace(title))
                return title;
        }
        catch (JsonException)
        {
            // Gövde ProblemDetails değil (ör. ağ geçidi hatası)
        }

        return statusCode switch
        {
            401 => "Oturumunuzun süresi doldu, tekrar giriş yapın.",
            403 => "Bu işlem için yetkiniz yok.",
            404 => "Kayıt bulunamadı.",
            _ => $"Beklenmeyen bir hata oluştu ({statusCode})."
        };
    }
}
