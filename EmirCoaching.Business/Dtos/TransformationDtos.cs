using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record TransformationMetricResponse
{
    public int Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

public record TransformationResponse
{
    public int Id { get; init; }
    public string ClientName { get; init; } = string.Empty;
    public int Age { get; init; }
    public int DurationWeeks { get; init; }
    public string TransformationTypeName { get; init; } = string.Empty;
    public string TransformationTypeSlug { get; init; } = string.Empty; // data-category
    public TransformationSection Section { get; init; }
    public string Quote { get; init; } = string.Empty;
    public string BeforeImageUrl { get; init; } = string.Empty;
    public string AfterImageUrl { get; init; } = string.Empty;
    public string? BeforeSquareUrl { get; init; }
    public string? AfterSquareUrl { get; init; }
    public int Year { get; init; }
    public bool AlignBottom { get; init; }
    public IReadOnlyList<TransformationMetricResponse> Metrics { get; init; } = [];

    // Admin düzenleme formu için
    public int TransformationTypeId { get; init; }
    public bool ShowOnHome { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
}

public record TransformationDetailResponse : TransformationResponse
{
    public string Story { get; init; } = string.Empty;
}

// Id: güncellemede var olan ölçümü numarası değişmeden günceller; boşsa yeni ölçüm eklenir.
// Listede olmayan eski ölçüm silinir. Oluşturmada Id yok sayılır.
public record TransformationMetricRequest
{
    public int? Id { get; init; }
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

// Metrics liste sırası SortOrder olarak kaydedilir
public record CreateTransformationRequest
{
    public string ClientName { get; init; } = string.Empty;
    public int Age { get; init; }
    public int DurationWeeks { get; init; }
    public int TransformationTypeId { get; init; }
    public TransformationSection Section { get; init; }
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
    public bool IsActive { get; init; }
    public List<TransformationMetricRequest> Metrics { get; init; } = [];
}

public record UpdateTransformationRequest : CreateTransformationRequest, IUpdateRequest
{
    public int Id { get; init; }
}
