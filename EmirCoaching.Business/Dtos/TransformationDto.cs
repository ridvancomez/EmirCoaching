using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record TransformationMetricDto
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

public record TransformationDto
{
    public int Id { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public int Age { get; init; }
    public int DurationWeeks { get; init; }
    public TransformationCategory Category { get; init; }
    public string Quote { get; init; } = string.Empty;
    public string BeforeImagePath { get; init; } = string.Empty;
    public string AfterImagePath { get; init; } = string.Empty;
    public string? BeforeSquarePath { get; init; }
    public string? AfterSquarePath { get; init; }
    public int Year { get; init; }
    public bool AlignBottom { get; init; }
    public IReadOnlyList<TransformationMetricDto> Metrics { get; init; } = [];
}

public record TransformationDetailDto : TransformationDto
{
    public string Story { get; init; } = string.Empty;
}

// Metrics liste sırası SortOrder olarak kaydedilir
public record CreateTransformationDto
{
    public string ClientName { get; init; } = string.Empty;
    public int Age { get; init; }
    public int DurationWeeks { get; init; }
    public TransformationCategory Category { get; init; }
    public string Quote { get; init; } = string.Empty;
    public string Story { get; init; } = string.Empty;
    public string BeforeImagePath { get; init; } = string.Empty;
    public string AfterImagePath { get; init; } = string.Empty;
    public string? BeforeSquarePath { get; init; }
    public string? AfterSquarePath { get; init; }
    public int Year { get; init; }
    public bool AlignBottom { get; init; }
    public bool ShowOnHome { get; init; }
    public int SortOrder { get; init; }
    public List<TransformationMetricDto> Metrics { get; init; } = [];
}

public record UpdateTransformationDto : CreateTransformationDto
{
    public int Id { get; init; }
}
