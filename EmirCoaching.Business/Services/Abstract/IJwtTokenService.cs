using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Identity;

namespace EmirCoaching.Business.Services.Abstract;

public interface IJwtTokenService
{
    AccessToken CreateAccessToken(AppUser user, IEnumerable<string> roles);

    // Rastgele, tahmin edilemez değer; DB'ye yalnızca özeti (Hash) yazılır
    string CreateRefreshToken();
    string HashRefreshToken(string refreshToken);
}
