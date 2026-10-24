using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class ContactMessage : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Message { get; set; }
    public ContactMessageStatus Status { get; set; }

    public int? PackageId { get; set; }
    public Package? Package { get; set; }

    public ICollection<ConsentRecord> ConsentRecords { get; set; } = new List<ConsentRecord>();
}
