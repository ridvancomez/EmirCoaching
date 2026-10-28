using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmirCoaching.Business.Services.Concrete;

public class AuthService(
    UserManager<AppUser> userManager,
    IUnitOfWork unitOfWork,
    IJwtTokenService tokenService,
    IMapper mapper,
    IOptions<JwtOptions> options,
    IValidator<LoginRequest> loginValidator,
    IValidator<UpdateProfileRequest> profileValidator,
    IValidator<ChangePasswordRequest> passwordValidator,
    IUploadCleanupService uploadCleanup,
    ILogger<AuthService> logger) : IAuthService
{
    // Kullanıcının var olup olmadığı belli edilmez: e-posta ve şifre hatası aynı mesajı alır
    private const string InvalidCredentials = "E-posta veya şifre hatalı.";
    private const string UserNotFound = "Kullanıcı bulunamadı.";
    private const string SessionExpired = "Oturumun süresi doldu. Lütfen tekrar giriş yapın.";
    private const int UserAgentMaxLength = 512;

    public async Task<Result<AuthTokens>> LoginAsync(LoginRequest request, RequestInfo requestInfo)
    {
        var errors = await loginValidator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result<AuthTokens>.Invalid(errors);

        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            await RecordAttemptAsync(LoginAttemptResult.KullaniciYok, null, requestInfo);
            logger.LogWarning("Giriş reddedildi: kullanıcı bulunamadı");
            return Result<AuthTokens>.Unauthorized(InvalidCredentials);
        }

        if (await userManager.IsLockedOutAsync(user))
        {
            await RecordAttemptAsync(LoginAttemptResult.Kilitli, user.Id, requestInfo);
            logger.LogWarning("Giriş reddedildi: kullanıcı {Id} kilitli", user.Id);
            return Result<AuthTokens>.Unauthorized("Çok fazla hatalı deneme yapıldı. Lütfen birkaç dakika sonra tekrar deneyin.");
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            await userManager.AccessFailedAsync(user);
            await RecordAttemptAsync(LoginAttemptResult.HataliSifre, user.Id, requestInfo);
            logger.LogWarning("Giriş reddedildi: kullanıcı {Id} hatalı şifre", user.Id);
            return Result<AuthTokens>.Unauthorized(InvalidCredentials);
        }

        await userManager.ResetAccessFailedCountAsync(user);

        var tokens = await IssueTokensAsync(user, requestInfo);
        await RecordAttemptAsync(LoginAttemptResult.Basarili, user.Id, requestInfo);

        logger.LogInformation("Kullanıcı {Id} giriş yaptı", user.Id);
        return Result<AuthTokens>.Success(tokens);
    }

    public async Task<Result<AuthTokens>> RefreshAsync(string? refreshToken, RequestInfo requestInfo)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return Result<AuthTokens>.Unauthorized(SessionExpired);

        var stored = await unitOfWork.RefreshTokens.GetByHashAsync(tokenService.HashRefreshToken(refreshToken));
        if (stored is null || stored.ExpiresAt <= DateTime.UtcNow)
            return Result<AuthTokens>.Unauthorized(SessionExpired);

        // İptal edilmiş token yeniden geldi: çalınmış olabilir, kullanıcının bütün oturumları kapatılır
        if (stored.RevokedAt is not null)
        {
            await RevokeAllAsync(stored.UserId);
            await unitOfWork.SaveChangesAsync();

            logger.LogWarning("Kullanıcı {Id} için iptal edilmiş refresh token kullanıldı; tüm oturumlar kapatıldı", stored.UserId);
            return Result<AuthTokens>.Unauthorized(SessionExpired);
        }

        stored.RevokedAt = DateTime.UtcNow;
        var tokens = await IssueTokensAsync(stored.User, requestInfo);
        await unitOfWork.SaveChangesAsync();

        return Result<AuthTokens>.Success(tokens);
    }

    public async Task<Result> LogoutAsync(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            return Result.Success();

        var stored = await unitOfWork.RefreshTokens.GetByHashAsync(tokenService.HashRefreshToken(refreshToken));
        if (stored is { RevokedAt: null })
        {
            stored.RevokedAt = DateTime.UtcNow;
            await unitOfWork.SaveChangesAsync();

            logger.LogInformation("Kullanıcı {Id} çıkış yaptı", stored.UserId);
        }

        return Result.Success();
    }

    public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync(int userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<CurrentUserResponse>.NotFound(UserNotFound);

        var roles = await userManager.GetRolesAsync(user);
        return Result<CurrentUserResponse>.Success(mapper.Map<CurrentUserResponse>(user) with { Roles = roles.ToList() });
    }

    public async Task<Result> UpdateProfileAsync(int userId, UpdateProfileRequest request)
    {
        var errors = await profileValidator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result.NotFound(UserNotFound);

        var oldUploads = UploadPaths.Of(user);
        mapper.Map(request, user);

        var updated = await userManager.UpdateAsync(user);
        if (!updated.Succeeded)
            return Result.Invalid(updated.Errors.Select(e => e.Description));

        await uploadCleanup.DeleteUnusedAsync(oldUploads.Except(UploadPaths.Of(user)));

        logger.LogInformation("Kullanıcı {Id} profilini güncelledi", user.Id);
        return Result.Success();
    }

    public async Task<Result<AuthTokens>> ChangePasswordAsync(int userId, ChangePasswordRequest request, RequestInfo requestInfo)
    {
        var errors = await passwordValidator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result<AuthTokens>.Invalid(errors);

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<AuthTokens>.NotFound(UserNotFound);

        // Mevcut şifre yanlışsa veya yeni şifre kurallara uymuyorsa Identity'nin (Türkçe) mesajları döner
        var changed = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!changed.Succeeded)
        {
            logger.LogWarning("Kullanıcı {Id} şifre değiştirme reddedildi", user.Id);
            return Result<AuthTokens>.Invalid(changed.Errors.Select(e => e.Description));
        }

        // Başka cihazlarda açık oturumlar kapanır (access token'ları süresi dolana kadar, en fazla 15 dk geçerli kalır)
        await RevokeAllAsync(user.Id);
        var tokens = await IssueTokensAsync(user, requestInfo);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Kullanıcı {Id} şifresini değiştirdi", user.Id);
        return Result<AuthTokens>.Success(tokens);
    }

    // Yeni access token + refresh token; refresh token'ın özeti kaydedilir (SaveChanges çağırana ait)
    private async Task<AuthTokens> IssueTokensAsync(AppUser user, RequestInfo requestInfo)
    {
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = tokenService.CreateAccessToken(user, roles);

        var refreshToken = tokenService.CreateRefreshToken();
        var refreshExpiresAt = DateTime.UtcNow.AddDays(options.Value.RefreshTokenDays);
        await unitOfWork.RefreshTokens.AddAsync(new RefreshToken
        {
            TokenHash = tokenService.HashRefreshToken(refreshToken),
            ExpiresAt = refreshExpiresAt,
            CreatedByIp = requestInfo.IpAddress,
            UserId = user.Id
        });

        return new AuthTokens(accessToken, refreshToken, refreshExpiresAt);
    }

    // Giriş geçmişi: şifre ve bulunamayan e-posta yazılmaz. Bekleyen değişiklikleri de (ör. yeni refresh token) kaydeder.
    private async Task RecordAttemptAsync(LoginAttemptResult result, int? userId, RequestInfo requestInfo)
    {
        await unitOfWork.LoginAttempts.AddAsync(new LoginAttempt
        {
            Result = result,
            IpAddress = requestInfo.IpAddress,
            UserAgent = requestInfo.UserAgent[..Math.Min(requestInfo.UserAgent.Length, UserAgentMaxLength)],
            UserId = userId
        });
        await unitOfWork.SaveChangesAsync();
    }

    private async Task RevokeAllAsync(int userId)
    {
        var now = DateTime.UtcNow;
        foreach (var token in await unitOfWork.RefreshTokens.GetActiveByUserIdAsync(userId))
            token.RevokedAt = now;
    }
}
