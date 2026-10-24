using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

// İletişim modalı (contact-modal) girdisi
public record CreateContactMessageDto
{
    public string Name { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public int? PackageId { get; init; }
    public string? Location { get; init; } // "evde" | "salonda"
    public string? Message { get; init; }
    public bool PrivacyAccepted { get; init; } // contact-consent
}

public record ContactMessageDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? Location { get; init; }
    public string? Message { get; init; }
    public ContactMessageStatus Status { get; init; }
    public int? PackageId { get; init; }
    public string? PackageName { get; init; }
    public DateTime CreatedAt { get; init; }
}
