using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Identity;

/// <summary>
/// Admin oturumunu yenilemek için verilen refresh token. Token'ın kendisi yalnızca istemcide
/// (HttpOnly cookie) durur; burada SHA-256 özeti tutulur. Her kullanımda iptal edilip yenisi verilir.
/// </summary>
public class RefreshToken : BaseEntity
{
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;

    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
}
