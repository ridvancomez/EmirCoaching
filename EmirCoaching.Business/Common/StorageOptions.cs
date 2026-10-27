namespace EmirCoaching.Business.Common;

// appsettings "Storage" bölümü
public class StorageOptions
{
    public const string SectionName = "Storage";

    // Yüklenen dosyalar (FileStorageService) API'nin wwwroot'unda bu önekle durur
    public const string UploadsPrefix = "/uploads/";

    // API'nin dışarıdan erişilen adresi (ör. https://api.emircoaching.com); boşsa yol göreli döner
    public string PublicBaseUrl { get; set; } = string.Empty;

    // Yalnızca yüklenen dosyalar (/uploads/...) API adresini alır; sitenin kendi görselleri (/images/...) göreli kalır
    public string ToPublicUrl(string path) =>
        string.IsNullOrEmpty(PublicBaseUrl) || !path.StartsWith(UploadsPrefix, StringComparison.OrdinalIgnoreCase)
            ? path
            : PublicBaseUrl.TrimEnd('/') + path;

    // Tersi: admin paneli yanıttan aldığı tam URL'yi (http://.../uploads/x.jpg) geri gönderebilir; API adresi kırpılır.
    // Başka adresler olduğu gibi kalır ve doğrulamada (SitePath) reddedilir.
    public string? ToStoredPath(string? value)
    {
        var uploadsUrl = PublicBaseUrl.TrimEnd('/') + UploadsPrefix;
        return value is not null && !string.IsNullOrEmpty(PublicBaseUrl) && value.StartsWith(uploadsUrl, StringComparison.OrdinalIgnoreCase)
            ? value[(uploadsUrl.Length - UploadsPrefix.Length)..]
            : value;
    }
}
