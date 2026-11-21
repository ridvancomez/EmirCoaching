namespace EmirCoaching.Admin.Services;

// Ad yazılırken slug kendiliğinden dolar; kullanıcı slug'ı elle değiştirdiyse dokunulmaz
public static class SlugField
{
    // oldName: değişmeden önceki ad, currentSlug: alandaki slug, newName: yeni ad
    public static string Follow(string? oldName, string? currentSlug, string? newName) =>
        string.IsNullOrEmpty(currentSlug) || currentSlug == SlugHelper.From(oldName)
            ? SlugHelper.From(newName)
            : currentSlug;
}
