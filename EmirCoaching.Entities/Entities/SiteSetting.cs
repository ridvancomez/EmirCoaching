using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

// Tek satırlık tablo (Id = 1)
public class SiteSetting : BaseEntity
{
    public string WhatsApp { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? Instagram { get; set; }
    public string? Facebook { get; set; }
    public string? LinkedIn { get; set; }
    public string? Twitter { get; set; }
    public string WorkingHours { get; set; } = string.Empty;
}
