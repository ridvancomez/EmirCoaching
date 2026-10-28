using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class MaintenanceServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<ILoginAttemptRepository> _loginAttempts = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokens = new();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly MaintenanceService _service;

    public MaintenanceServiceTests()
    {
        _uow.Setup(u => u.LoginAttempts).Returns(_loginAttempts.Object);
        _uow.Setup(u => u.RefreshTokens).Returns(_refreshTokens.Object);
        _service = new MaintenanceService(_uow.Object, _uploadCleanup.Object, NullLogger<MaintenanceService>.Instance);
    }

    // Verilen tarih "şimdi - süre"ye bir dakika içinde yakın mı
    private static bool IsAgo(DateTime value, TimeSpan age) =>
        (DateTime.UtcNow - age - value).Duration() < TimeSpan.FromMinutes(1);

    [Fact]
    public async Task RunAsync_DeletesUploadsOlderThan24Hours()
    {
        // Act
        await _service.RunAsync();

        // Assert — daha yeni yüklemeler panelde kaydedilmeyi bekliyor olabilir
        _uploadCleanup.Verify(c => c.DeleteOrphansAsync(It.Is<DateTime>(d => IsAgo(d, TimeSpan.FromHours(24)))), Times.Once);
    }

    [Fact]
    public async Task RunAsync_DeletesLoginHistoryOlderThan90Days()
    {
        // Act
        await _service.RunAsync();

        // Assert
        _loginAttempts.Verify(r => r.DeletePermanentlyBeforeAsync(It.Is<DateTime>(d => IsAgo(d, TimeSpan.FromDays(90)))), Times.Once);
    }

    [Fact]
    public async Task RunAsync_DeletesRefreshTokensExpiredMoreThan30DaysAgo()
    {
        // Act
        await _service.RunAsync();

        // Assert — iptal edilenler süresi dolana kadar tutulur ki tekrar kullanım yakalansın
        _refreshTokens.Verify(r => r.DeletePermanentlyExpiredBeforeAsync(It.Is<DateTime>(d => IsAgo(d, TimeSpan.FromDays(30)))), Times.Once);
    }
}
