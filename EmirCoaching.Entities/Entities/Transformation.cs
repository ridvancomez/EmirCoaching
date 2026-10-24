using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class Transformation : BaseEntity
{
    public string ClientName { get; set; } = string.Empty;
    public int Age { get; set; }
    public int DurationWeeks { get; set; }
    public TransformationCategory Category { get; set; }
    public string Quote { get; set; } = string.Empty;
    public string Story { get; set; } = string.Empty;
    public string BeforeImagePath { get; set; } = string.Empty;
    public string AfterImagePath { get; set; } = string.Empty;
    public string? BeforeSquarePath { get; set; }
    public string? AfterSquarePath { get; set; }
    public int Year { get; set; }         // data-yil
    public bool AlignBottom { get; set; } // compare--alta-yasli
    public bool ShowOnHome { get; set; }
    public int SortOrder { get; set; }

    public ICollection<TransformationMetric> Metrics { get; set; } = new List<TransformationMetric>();
}
