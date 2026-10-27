namespace EmirCoaching.Business.Dtos;

public record SiteSettingResponse
{
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string LegalEmail { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string WorkingDays1 { get; init; } = string.Empty;
    public string WorkingHours1 { get; init; } = string.Empty;
    public string WorkingDays2 { get; init; } = string.Empty;
    public string WorkingHours2 { get; init; } = string.Empty;
}

// Tek satırlık ayar kaydı: Id yok, her zaman aynı kayıt güncellenir
public record UpdateSiteSettingRequest
{
    public string Phone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string LegalEmail { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string WorkingDays1 { get; init; } = string.Empty;
    public string WorkingHours1 { get; init; } = string.Empty;
    public string WorkingDays2 { get; init; } = string.Empty;
    public string WorkingHours2 { get; init; } = string.Empty;
}
