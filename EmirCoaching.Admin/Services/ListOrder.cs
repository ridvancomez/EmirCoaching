namespace EmirCoaching.Admin.Services;

// Formdaki alt listeler (paket modülleri, ölçümler): API liste sırasını SortOrder olarak kaydeder
public static class ListOrder
{
    public static void MoveUp<T>(List<T> list, T item)
    {
        var index = list.IndexOf(item);
        if (index > 0)
            (list[index - 1], list[index]) = (list[index], list[index - 1]);
    }

    public static void MoveDown<T>(List<T> list, T item)
    {
        var index = list.IndexOf(item);
        if (index >= 0 && index < list.Count - 1)
            (list[index + 1], list[index]) = (list[index], list[index + 1]);
    }
}
