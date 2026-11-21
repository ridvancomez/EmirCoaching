using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

// "Nasıl Çalışır?" adımları programdan bağımsızdır; kendi uçlarından yönetilir (diet-program-steps)
public record DietProgramStepResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DietProgramStepIcon Icon { get; init; }
    public string IconName { get; init; } = string.Empty; // <iconify-icon icon="..."> için, ör. "mdi:clipboard-text"
    public int SortOrder { get; init; }
}

public record CreateDietProgramStepRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DietProgramStepIcon Icon { get; init; } // 1 FormuDoldur, 2 PlanHazirlanir, 3 WhatsApp
    public int SortOrder { get; init; }
}

public record UpdateDietProgramStepRequest : CreateDietProgramStepRequest, IUpdateRequest
{
    public int Id { get; init; }
}

public record DietProgramResponse
{
    public string Title { get; init; } = string.Empty;
    public string DietitianTitle { get; init; } = string.Empty;
    public string DietitianFullName { get; init; } = string.Empty;
    public string? DietitianPhotoUrl { get; init; }
    public bool IsActive { get; init; }
}

// Tek kayıt: Id yok
public record UpdateDietProgramRequest
{
    public string Title { get; init; } = string.Empty;
    public string DietitianTitle { get; init; } = string.Empty;
    public string DietitianFullName { get; init; } = string.Empty;
    public string? DietitianPhotoPath { get; init; }
    public bool IsActive { get; init; }
}
