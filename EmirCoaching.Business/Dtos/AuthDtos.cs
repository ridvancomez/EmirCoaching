namespace EmirCoaching.Business.Dtos;

public record LoginRequest
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public record AccessToken(string Token, DateTime ExpiresAt);

// Servisin ürettiği token çifti. API refresh token'ı HttpOnly cookie'ye yazar, yanıtta yalnızca AuthResponse döner.
public record AuthTokens(AccessToken AccessToken, string RefreshToken, DateTime RefreshTokenExpiresAt);

public record AuthResponse
{
    public string AccessToken { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; } // UTC; istemci bundan önce /auth/refresh çağırır
}

public record CurrentUserResponse
{
    public int Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public IReadOnlyList<string> Roles { get; init; } = [];
}
