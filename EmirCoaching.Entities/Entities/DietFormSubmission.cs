using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class DietFormSubmission : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal HeightCm { get; set; }
    public decimal CurrentWeight { get; set; }
    public decimal TargetWeight { get; set; }
    public int MealsPerDay { get; set; }
    public string? Allergies { get; set; }
    public string? AvoidedFoods { get; set; }
    public string? Supplements { get; set; }
    public int? SleepHours { get; set; }
    public ActivityLevel ActivityLevel { get; set; }
    public decimal? WaterLiters { get; set; }
    public DietFormStatus Status { get; set; }

    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
