using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record CreateDietFormSubmissionRequest
{
    public int PackageId { get; init; } // aktif bir paket olmalı
    public string FullName { get; init; } = string.Empty;
    public Gender Gender { get; init; }
    public int Age { get; init; }
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public decimal HeightCm { get; init; }
    public decimal CurrentWeight { get; init; }
    public decimal TargetWeight { get; init; }
    public bool PrivacyAccepted { get; init; }

    // Sağlık verileri için açık rıza (işaretlenirse kaydedilir)
    public bool HealthDataConsent { get; init; }
}

public record DietFormSubmissionResponse
{
    public int Id { get; init; }
    public int PackageId { get; init; }
    public string PackageName { get; init; } = string.Empty;
    public PackageLocation PackageLocation { get; init; }
    public bool PackageIsDeleted { get; init; } // panelde soluk ve tıklanamaz gösterilir
    public string FullName { get; init; } = string.Empty;
    public Gender Gender { get; init; }
    public int Age { get; init; }
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public decimal HeightCm { get; init; }
    public decimal CurrentWeight { get; init; }
    public decimal TargetWeight { get; init; }
    public string? ClientFormUrl { get; init; } // boşsa danışan Google Form'u henüz doldurmadı
    public DietFormStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
}

// Admin: başvurunun durumunu değiştirir. Yeni dışındaki durumlarda kişi yeniden başvurabilir.
public record UpdateDietFormSubmissionStatusRequest
{
    public DietFormStatus Status { get; init; }
}

// Admin: danışanın Google Form yanıt linki; boş gönderilirse link silinir
public record UpdateDietFormSubmissionClientFormRequest
{
    public string? ClientFormUrl { get; init; }
}
