using System.Text.Json;

namespace EmirCoaching.Admin.Services;

// Response → Create/Update isteği: NSwag tipleri aynı JSON adlarını taşır, eşleşen alanlar kopyalanır.
// Adı farklı alanlar (yanıtta *Url, istekte *Path) sayfada elle atanır.
public static class ObjectCopy
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static T To<T>(object source) =>
        JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(source, source.GetType(), Options), Options)!;
}
