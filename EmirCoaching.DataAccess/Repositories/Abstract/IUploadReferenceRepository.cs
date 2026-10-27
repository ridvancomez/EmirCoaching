namespace EmirCoaching.DataAccess.Repositories.Abstract;

// Bir entity'ye bağlı değil: yüklenen dosyaya (/uploads/...) işaret eden tüm kolonları tek sorguda okur.
// Yeni bir *Path kolonu eklenince buraya da eklenir; eklenmezse o kolonun dosyası temizlikte silinir.
public interface IUploadReferenceRepository
{
    // Silinmemiş kayıtların kullandığı yüklemeler (yasal metin PDF'leri: eski sürümler dahil hepsi)
    Task<HashSet<string>> GetUsedPathsAsync();
}
