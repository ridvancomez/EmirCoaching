using EmirCoaching.Business.Services.Abstract;

namespace EmirCoaching.API.BackgroundJobs;

// Uygulama açıldıktan kısa süre sonra ve ardından günde bir kez IMaintenanceService'i çalıştırır.
// Hata uygulamayı durdurmaz: Error olarak loglanır, sonraki gün yeniden denenir.
public class MaintenanceWorker(IServiceScopeFactory scopeFactory, ILogger<MaintenanceWorker> logger) : BackgroundService
{
    private static readonly TimeSpan StartupDelay = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan Interval = TimeSpan.FromDays(1);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Delay(StartupDelay, stoppingToken);

            using var timer = new PeriodicTimer(Interval);
            do
            {
                await RunOnceAsync();
            } while (await timer.WaitForNextTickAsync(stoppingToken));
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // uygulama kapanıyor
        }
    }

    private async Task RunOnceAsync()
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            await scope.ServiceProvider.GetRequiredService<IMaintenanceService>().RunAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Günlük bakım başarısız oldu");
        }
    }
}
