using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Modül kartındaki tek kalem (örn. "Haftalık Program Güncellemesi").
/// Kilitli kalem modül sayısına ve "Toplam 15 kalem" toplamına dahil edilmez.
/// </summary>
public class PackageModuleItem : BaseEntity, ISortable
{
    public string Text { get; set; } = string.Empty;
    public bool IsLocked { get; set; } // modul-card__item--kilitli
    public int SortOrder { get; set; }

    public int ModuleId { get; set; }
    public PackageModule Module { get; set; } = null!;
}
