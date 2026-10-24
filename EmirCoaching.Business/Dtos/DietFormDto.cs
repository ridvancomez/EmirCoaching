using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record CreateDietFormDto
{
    public string FullName { get; init; } = string.Empty;
    public int Age { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public decimal HeightCm { get; init; }
    public decimal CurrentWeight { get; init; }
    public decimal TargetWeight { get; init; }
    public int MealsPerDay { get; init; }
    public string? Allergies { get; init; }
    public string? AvoidedFoods { get; init; }
    public string? Supplements { get; init; }
    public int? SleepHours { get; init; }
    public ActivityLevel ActivityLevel { get; init; }
    public decimal? WaterLiters { get; init; }

    public bool PrivacyAccepted { get; init; }
    // Sağlık verileri için açık rıza (müşteri onayı bekleniyor; işaretlenirse kaydedilir)
    public bool HealthDataConsent { get; init; }
}

public record DietFormDto
{
    public int Id { get; init; }
    public string FullName { get; init; } = string.Empty;
    public int Age { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public decimal HeightCm { get; init; }
    public decimal CurrentWeight { get; init; }
    public decimal TargetWeight { get; init; }
    public int MealsPerDay { get; init; }
    public string? Allergies { get; init; }
    public string? AvoidedFoods { get; init; }
    public string? Supplements { get; init; }
    public int? SleepHours { get; init; }
    public ActivityLevel ActivityLevel { get; init; }
    public decimal? WaterLiters { get; init; }
    public DietFormStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}
