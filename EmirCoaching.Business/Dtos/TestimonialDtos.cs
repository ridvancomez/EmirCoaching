using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

public record TestimonialResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public decimal Rating { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public int PackageId { get; init; }
}

public record CreateTestimonialRequest
{
    public string Name { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public decimal Rating { get; init; } // 0–5, 0.5 adımlarla
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public int PackageId { get; init; } // yorum bu paketin detay sayfasında gösterilir
}

public record UpdateTestimonialRequest : CreateTestimonialRequest, IUpdateRequest
{
    public int Id { get; init; }
}
