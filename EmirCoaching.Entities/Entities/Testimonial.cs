using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class Testimonial : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }

    public int? PackageId { get; set; }
    public Package? Package { get; set; }
}
