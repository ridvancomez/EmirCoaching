namespace EmirCoaching.Business.Services.Abstract;

// Kullanılmayan yüklemeleri siler. Bir dosya, silinmemiş hiçbir kayıt kullanmıyorsa kullanılmıyor sayılır
// (aynı görsel birden fazla kayıtta olabilir). Yasal metin PDF'leri eski sürümler dahil hiç silinmez.
public interface IUploadCleanupService
{
    // Kayıttan çıkan dosyalar (görseli değişen veya silinen kayıt); kaydetmeden SONRA çağrılır
    Task DeleteUnusedAsync(IEnumerable<string> paths);

    // Günlük temizlik: hiçbir kayda bağlı olmayan ve verilen tarihten eski dosyalar
    // (yüklenip kaydedilmeyen görseller dahil); silinen sayısı
    Task<int> DeleteOrphansAsync(DateTime modifiedBefore);
}
