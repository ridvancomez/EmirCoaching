using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class PackageHighlight : BaseEntity
{
    public string Text { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;
}
