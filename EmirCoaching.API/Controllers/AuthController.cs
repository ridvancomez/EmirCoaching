using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EmirCoaching.API.Controllers;

// Access token yanıt gövdesinde döner (istemci bellekte tutar, Authorization: Bearer ile gönderir).
// Refresh token HttpOnly cookie'dedir: JavaScript okuyamaz, yalnızca /api/v1/auth uçlarına gider.
[Route("api/v1/auth")]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    private const string RefreshCookieName = "refresh_token";
    private const string RefreshCookiePath = "/api/v1/auth";

    // → 200 { accessToken, expiresAt } + cookie / 400 / 401
    [HttpPost("login")]
    [EnableRateLimiting(RateLimitPolicies.Login)]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request) =>
        FromResult(await authService.LoginAsync(request, GetRequestInfo()), WriteTokens);

    // Cookie'deki refresh token ile yeni token çifti → 200 / 401
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponse>> Refresh() =>
        FromResult(await authService.RefreshAsync(Request.Cookies[RefreshCookieName], GetRequestInfo()), WriteTokens);

    // → 204; refresh token iptal edilir ve cookie silinir (access token süresi dolana kadar geçerli kalır)
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await authService.LogoutAsync(Request.Cookies[RefreshCookieName]);
        Response.Cookies.Delete(RefreshCookieName, RefreshCookieOptions(expiresAt: null));
        return FromResult(result);
    }

    // Giriş yapmış kullanıcı (admin paneli üst bilgisi) → 200 / 401
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> Me() =>
        FromResult(await authService.GetCurrentUserAsync(CurrentUserId));

    // Kullanıcı ayarları: ad, unvan, biyografi, fotoğraf (blog yazar kartı) → 204 / 400 / 401
    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request) =>
        FromResult(await authService.UpdateProfileAsync(CurrentUserId, request));

    // Diğer oturumlar kapanır, bu oturum yeni token çiftiyle devam eder → 200 { accessToken, expiresAt } + cookie / 400 / 401 / 429
    [Authorize]
    [HttpPost("change-password")]
    [EnableRateLimiting(RateLimitPolicies.Login)] // mevcut şifre denemesi kaba kuvvete açık olmasın
    public async Task<ActionResult<AuthResponse>> ChangePassword(ChangePasswordRequest request) =>
        FromResult(await authService.ChangePasswordAsync(CurrentUserId, request, GetRequestInfo()), WriteTokens);

    private ActionResult<AuthResponse> WriteTokens(AuthTokens tokens)
    {
        Response.Cookies.Append(RefreshCookieName, tokens.RefreshToken, RefreshCookieOptions(tokens.RefreshTokenExpiresAt));
        return Ok(new AuthResponse { AccessToken = tokens.AccessToken.Token, ExpiresAt = tokens.AccessToken.ExpiresAt });
    }

    // SameSite=Strict: başka bir siteden gelen istekte cookie gönderilmez (CSRF koruması).
    // Site ve API aynı site altında olmalı (localhost:5218 ↔ localhost:5164, emircoaching.com ↔ api.emircoaching.com).
    private static CookieOptions RefreshCookieOptions(DateTime? expiresAt) => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Path = RefreshCookiePath,
        Expires = expiresAt,
        IsEssential = true
    };
}
