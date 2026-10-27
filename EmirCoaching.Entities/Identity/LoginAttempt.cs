using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Identity;

/// <summary>
/// Admin paneline giriş denemesi (başarılı/başarısız). Denemenin zamanı CreatedAt'tir.
/// Girilen şifre ve bulunamayan e-posta asla tutulmaz; kullanıcı yalnızca eşleştiyse bağlanır.
/// Saklama süresi dolunca günlük bakım işiyle kalıcı silinir.
/// </summary>
public class LoginAttempt : BaseEntity
{
    public LoginAttemptResult Result { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;

    public int? UserId { get; set; }
    public AppUser? User { get; set; }
}
