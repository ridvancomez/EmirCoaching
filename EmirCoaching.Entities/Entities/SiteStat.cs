using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class SiteStat : BaseEntity
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string? Suffix { get; set; }
    public int SortOrder { get; set; }
}
