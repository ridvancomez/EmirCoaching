namespace EmirCoaching.Business.Services.Abstract;

// Günlük bakım (API'deki MaintenanceWorker çağırır): kullanılmayan yüklemeler, eski giriş geçmişi,
// süresi çoktan dolmuş oturum kayıtları kalıcı silinir.
public interface IMaintenanceService
{
    Task RunAsync();
}
