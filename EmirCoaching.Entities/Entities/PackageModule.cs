using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class PackageModule : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string IconKey { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;

    public ICollection<PackageModuleItem> Items { get; set; } = new List<PackageModuleItem>();
}
