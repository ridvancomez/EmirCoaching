namespace EmirCoaching.Business.Common;

// Toplu silme (POST .../bulk-delete, gövde: [3, 7, 12]) ortak kuralları. Ya hepsi ya hiçbiri:
// kayıtlardan biri yoksa veya silinemiyorsa hiçbiri silinmez.
public static class BulkDelete
{
    public const int MaxCount = 100;

    public static Result Validate(IReadOnlyCollection<int> ids) => ids.Count switch
    {
        0 => Result.Invalid("Silinecek kayıt seçilmedi."),
        > MaxCount => Result.Invalid($"Tek seferde en fazla {MaxCount} kayıt silinebilir."),
        _ when ids.Any(id => id <= 0) => Result.Invalid("Geçersiz kayıt numarası."),
        _ => Result.Success()
    };

    // Seçilen kayıtları yükler; biri bile bulunamazsa NotFound döner (tekrar eden numara bir kez yüklenir)
    public static async Task<Result<List<T>>> LoadAsync<T>(IReadOnlyCollection<int> ids, Func<int, Task<T?>> load, string notFound)
        where T : class
    {
        var check = Validate(ids);
        if (!check.IsSuccess)
            return Result<List<T>>.Failure(check);

        var entities = new List<T>();
        foreach (var id in ids.Distinct())
        {
            var entity = await load(id);
            if (entity is null)
                return Result<List<T>>.NotFound(notFound);
            entities.Add(entity);
        }

        return Result<List<T>>.Success(entities);
    }
}
