using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

// İletişim modalı (contact-modal) girdisi
public record CreateContactMessageRequest
{
    public string Name { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int? PackageId { get; init; }
    public string? Location { get; init; } // front-end değeri: "evde" | "salonda" (PackageLocation'a çevrilir)
    public string? Message { get; init; }
    public bool PrivacyAccepted { get; init; } // contact-consent
}

public record ContactMessageResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public PackageLocation? Location { get; init; }
    public string? Message { get; init; }
    public ContactMessageStatus Status { get; init; }
    public int? PackageId { get; init; }
    public string? PackageName { get; init; }
    public DateTime CreatedAt { get; init; }
}

// Admin: mesajın durumunu değiştirir (Yeni → Okundu → Donuldu)
public record UpdateContactMessageStatusRequest
{
    public ContactMessageStatus Status { get; init; }
}
