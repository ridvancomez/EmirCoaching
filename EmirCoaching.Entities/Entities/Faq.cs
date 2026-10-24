using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class Faq : BaseEntity
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public FaqGroup Group { get; set; }
    public int SortOrder { get; set; }
}
