using System.Text.Json.Serialization.Metadata;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.API.Common;

// İstek okunurken Request DTO'larındaki her *Path alanı (CoverImagePath, Steps[].ImagePath...) StorageOptions.ToStoredPath'ten geçer:
// yanıttaki *Url değeri aynen geri gönderilebilir, DB'ye göreli yol yazılır. Doğrulama ve servisler yalnızca göreli yol görür.
public static class StoredPathJson
{
    private static readonly string DtoNamespace = typeof(LoginRequest).Namespace!;

    public static Action<JsonTypeInfo> Modifier(StorageOptions storage) => typeInfo =>
    {
        if (typeInfo.Kind != JsonTypeInfoKind.Object || typeInfo.Type.Namespace != DtoNamespace)
            return;

        foreach (var property in typeInfo.Properties)
        {
            if (property.PropertyType != typeof(string) || !property.Name.EndsWith("Path", StringComparison.Ordinal)
                || property.Set is not { } set)
                continue;

            property.Set = (target, value) => set(target, storage.ToStoredPath((string?)value));
        }
    };
}
