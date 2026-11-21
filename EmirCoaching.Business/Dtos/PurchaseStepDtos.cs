using EmirCoaching.Business.Common;

namespace EmirCoaching.Business.Dtos;

public record PurchaseStepResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string TimeLabel { get; init; } = string.Empty;
    public string IconKey { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}

public record CreatePurchaseStepRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string TimeLabel { get; init; } = string.Empty; // "Gün 1", "Her hafta"
    public string IconKey { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}

public record UpdatePurchaseStepRequest : CreatePurchaseStepRequest, IUpdateRequest
{
    public int Id { get; init; }
}
