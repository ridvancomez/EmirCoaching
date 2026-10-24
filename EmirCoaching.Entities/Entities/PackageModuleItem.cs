using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class PackageModuleItem : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public ModuleItemState State { get; set; }
    public string? LockedLabel { get; set; }
    public int SortOrder { get; set; }

    public int ModuleId { get; set; }
    public PackageModule Module { get; set; } = null!;
}
