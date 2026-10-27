using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

// Admin giriş geçmişi satırı; kullanıcı bilgisi yalnızca deneme bir hesapla eşleştiyse dolu
public record LoginAttemptResponse
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; } // deneme zamanı (UTC)
    public LoginAttemptResult Result { get; init; }
    public string IpAddress { get; init; } = string.Empty;
    public string UserAgent { get; init; } = string.Empty;
    public int? UserId { get; init; }
    public string? UserEmail { get; init; }
    public string? UserFullName { get; init; }
}
