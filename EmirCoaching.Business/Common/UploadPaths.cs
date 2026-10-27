using System.Reflection;

namespace EmirCoaching.Business.Common;

// Entity'nin yüklenen dosyaya işaret eden alanları: adı *Path ile biten ve /uploads/ ile başlayan string'ler.
// Değiştirilen veya silinen kaydın eski dosyalarını bulmak için kullanılır.
public static class UploadPaths
{
    private const string UploadsPrefix = "/uploads/";

    public static List<string> Of(object entity) =>
        PathProperties(entity.GetType())
            .Select(p => p.GetValue(entity) as string)
            .Where(v => v is not null && v.StartsWith(UploadsPrefix, StringComparison.OrdinalIgnoreCase))
            .Select(v => v!)
            .ToList();

    private static IEnumerable<PropertyInfo> PathProperties(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.PropertyType == typeof(string) && p.Name.EndsWith("Path", StringComparison.Ordinal) && p.CanRead);
}
