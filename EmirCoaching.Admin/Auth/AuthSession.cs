using EmirCoaching.Admin.Api;

namespace EmirCoaching.Admin.Auth;

// Oturum durumu: access token yalnızca bellekte durur (sayfa yenilenince refresh cookie'siyle geri alınır).
public class AuthSession
{
    public string? AccessToken { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public CurrentUserResponse? User { get; private set; }

    public bool IsAuthenticated => AccessToken is not null;

    // Süresine 30 sn'den az kalan token yenilenir: istek yoldayken dolmasın
    public bool IsExpiring => ExpiresAtUtc <= DateTime.UtcNow.AddSeconds(30);

    // Kullanıcı bilgisi değişince (profil kaydı, giriş/çıkış) üst menü yeniden çizilir
    public event Action? Changed;

    public void SetTokens(AuthResponse tokens)
    {
        AccessToken = tokens.AccessToken;
        ExpiresAtUtc = tokens.ExpiresAt.ToUniversalTime();
    }

    public void SetUser(CurrentUserResponse user)
    {
        User = user;
        Changed?.Invoke();
    }

    public void Clear()
    {
        AccessToken = null;
        User = null;
        Changed?.Invoke();
    }
}
