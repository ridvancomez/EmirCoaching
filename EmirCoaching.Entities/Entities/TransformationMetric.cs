using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class TransformationMetric : BaseEntity
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public int TransformationId { get; set; }
    public Transformation Transformation { get; set; } = null!;
}
