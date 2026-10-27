using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace EmirCoaching.Business.Common;

// appsettings "Jwt" bölümü; SecretKey yalnızca user-secrets / ortam değişkeninden gelir
public class JwtOptions
{
    public const string SectionName = "Jwt";
    public const int MinSecretKeyBytes = 32; // HMAC-SHA256 için en az 256 bit

    // Token'daki claim adları (API doğrulamada aynı adları kullanır)
    public const string NameClaimType = JwtRegisteredClaimNames.Name;
    public const string RoleClaimType = "role";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;

    public SymmetricSecurityKey GetSigningKey() => new(Encoding.UTF8.GetBytes(SecretKey));
}
