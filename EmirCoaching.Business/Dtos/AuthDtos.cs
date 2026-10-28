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

// Panel üst bilgisi ve kullanıcı ayarları formu
public record CurrentUserResponse
{
    public int Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string? Title { get; init; }
    public string? Bio { get; init; }
    public string? AvatarUrl { get; init; }
    public IReadOnlyList<string> Roles { get; init; } = [];
}

// Blog yazılarındaki yazar kartında görünür
public record UpdateProfileRequest
{
    public string FullName { get; init; } = string.Empty;
    public string? Title { get; init; }
    public string? Bio { get; init; }
    public string? AvatarPath { get; init; }
}

public record ChangePasswordRequest
{
    public string CurrentPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}
