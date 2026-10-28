using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;

namespace EmirCoaching.Business.Services.Abstract;

public interface IAuthService
{
    // Hatalı şifre sayılır; 5 hatada hesap geçici kilitlenir (Identity lockout)
    Task<Result<AuthTokens>> LoginAsync(LoginRequest request, RequestInfo requestInfo);

    // Refresh token tek kullanımlık: eskisi iptal edilir, yenisi verilir
    Task<Result<AuthTokens>> RefreshAsync(string? refreshToken, RequestInfo requestInfo);

    Task<Result> LogoutAsync(string? refreshToken);

    Task<Result<CurrentUserResponse>> GetCurrentUserAsync(int userId);

    Task<Result> UpdateProfileAsync(int userId, UpdateProfileRequest request);

    // Başarılıysa kullanıcının tüm oturumları kapatılır; bu oturum dönen yeni token'larla devam eder
    Task<Result<AuthTokens>> ChangePasswordAsync(int userId, ChangePasswordRequest request, RequestInfo requestInfo);
}
