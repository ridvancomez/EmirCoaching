using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class MaintenanceService(
    IUnitOfWork unitOfWork,
    IUploadCleanupService uploadCleanup,
    ILogger<MaintenanceService> logger) : IMaintenanceService
{
    private const int LoginHistoryDays = 90;
    private const int ExpiredRefreshTokenDays = 30;
    private const int OrphanUploadHours = 24; // panelde yüklenip henüz kaydedilmemiş görsele dokunulmaz

    public async Task RunAsync()
    {
        var now = DateTime.UtcNow;

        var uploads = await uploadCleanup.DeleteOrphansAsync(now.AddHours(-OrphanUploadHours));
        var attempts = await unitOfWork.LoginAttempts.DeletePermanentlyBeforeAsync(now.AddDays(-LoginHistoryDays));
        var tokens = await unitOfWork.RefreshTokens.DeletePermanentlyExpiredBeforeAsync(now.AddDays(-ExpiredRefreshTokenDays));

        logger.LogInformation(
            "Günlük bakım tamamlandı: {Uploads} kullanılmayan dosya, {Attempts} eski giriş kaydı, {Tokens} eski oturum kaydı silindi",
            uploads, attempts, tokens);
    }
}
