using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Common;

// Üst kaydın alt listesini (paket modülleri, değişim ölçümleri...) istekteki listeyle numaraları koruyarak eşitler.
public static class ChildListSync
{
    // Gönderilen Id'ler üst kayda ait olmalı ve tekrar etmemeli; boş Id yeni kayıt demektir
    public static bool AreKnownIds(IEnumerable<int?> sent, IEnumerable<int> known)
    {
        var ids = sent.OfType<int>().ToList();
        var knownIds = known.ToHashSet();
        return ids.Distinct().Count() == ids.Count && ids.All(knownIds.Contains);
    }

    // Id'si eşleşen güncellenir, eşleşmeyen eklenir, listede olmayan remove ile silinir. Liste sırası SortOrder'a verilir.
    public static void Sync<TEntity, TInput>(ICollection<TEntity> existing, List<TInput> inputs, Func<TInput, int?> idOf,
        Action<TEntity> remove, Action<TEntity, TInput, int> apply) where TEntity : BaseEntity, new()
    {
        var byId = existing.ToDictionary(e => e.Id);
        var kept = new HashSet<int>();

        for (var i = 0; i < inputs.Count; i++)
        {
            if (idOf(inputs[i]) is { } id && byId.TryGetValue(id, out var entity))
                kept.Add(id);
            else
                existing.Add(entity = new TEntity());

            apply(entity, inputs[i], i + 1);
        }

        foreach (var entity in byId.Values.Where(e => !kept.Contains(e.Id)))
            remove(entity);
    }
}
