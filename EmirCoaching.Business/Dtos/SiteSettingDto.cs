namespace EmirCoaching.Business.Dtos;

public record SiteSettingDto
{
    public string WhatsApp { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string? Instagram { get; init; }
    public string? Facebook { get; init; }
    public string? LinkedIn { get; init; }
    public string? Twitter { get; init; }
    public string WorkingHours { get; init; } = string.Empty;
}

// Tek satırlık ayar kaydı: Id yok, her zaman aynı kayıt güncellenir
public record UpdateSiteSettingDto : SiteSettingDto;
