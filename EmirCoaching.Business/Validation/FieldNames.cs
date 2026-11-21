namespace EmirCoaching.Business.Validation;

// Hata mesajlarında görünen Türkçe alan adları (DTO property adı → kullanıcıya gösterilen ad).
// FluentValidation (TurkishValidation) ve API'nin model binding mesajları (TurkishModelBinding) buradan okur.
// Yeni bir Request alanı eklenince buraya da eklenir; eklenmezse mesajda property adı görünür.
public static class FieldNames
{
    private static readonly Dictionary<string, string> Names = new(StringComparer.OrdinalIgnoreCase)
    {
        // Ortak
        ["Id"] = "Kayıt numarası",
        ["Name"] = "Ad",
        ["FullName"] = "Ad soyad",
        ["Email"] = "E-posta",
        ["Phone"] = "Telefon",
        ["PhoneNumber"] = "Telefon",
        ["Title"] = "Başlık",
        ["Subtitle"] = "Alt başlık",
        ["Description"] = "Açıklama",
        ["ShortDescription"] = "Kısa açıklama",
        ["Text"] = "Metin",
        ["Label"] = "Etiket",
        ["Value"] = "Değer",
        ["Slug"] = "URL adı",
        ["Url"] = "Bağlantı",
        ["SortOrder"] = "Sıra",
        ["IsActive"] = "Aktif",
        ["IsFeatured"] = "Öne çıkan",
        ["IsLocked"] = "Kilitli",
        ["MetaTitle"] = "Meta başlık",
        ["MetaDescription"] = "Meta açıklama",
        ["IconKey"] = "İkon",
        ["Icon"] = "İkon",
        ["ImagePath"] = "Görsel",
        ["Message"] = "Mesaj",
        ["Password"] = "Şifre",
        ["CurrentPassword"] = "Mevcut şifre",
        ["NewPassword"] = "Yeni şifre",
        ["Bio"] = "Biyografi",
        ["AvatarPath"] = "Fotoğraf",
        ["ModuleIds"] = "Modül sırası",
        ["Status"] = "Durum",
        ["File"] = "Dosya",
        ["Folder"] = "Klasör",
        ["Note"] = "Değişiklik notu",
        ["IsPublished"] = "Yayın durumu",
        ["Page"] = "Sayfa",
        ["Size"] = "Sayfa boyutu",

        // Paketler ve yorumlar
        ["PackageId"] = "Paket",
        ["Price"] = "Fiyat",
        ["DurationMonths"] = "Süre (ay)",
        ["Location"] = "Antrenman yeri",
        ["PurchaseUrl"] = "Satın alma bağlantısı",
        ["Highlights"] = "Öne çıkanlar",
        ["SubText"] = "Alt satır",
        ["Modules"] = "Modüller",
        ["Items"] = "Kalemler",
        ["Rating"] = "Puan",

        // Blog
        ["CategoryId"] = "Kategori",
        ["AuthorId"] = "Yazar",
        ["Summary"] = "Özet",
        ["ContentHtml"] = "İçerik",
        ["CoverImagePath"] = "Kapak görseli",
        ["CoverImageCaption"] = "Kapak görseli açıklaması",
        ["ReadingMinutes"] = "Okuma süresi",
        ["Tags"] = "Etiketler",

        // Değişimler
        ["TransformationTypeId"] = "Değişim türü",
        ["ClientName"] = "Danışan adı",
        ["Section"] = "Bölüm",
        ["Quote"] = "Alıntı",
        ["Story"] = "Hikâye",
        ["BeforeImagePath"] = "Önce fotoğrafı",
        ["AfterImagePath"] = "Sonra fotoğrafı",
        ["BeforeSquarePath"] = "Önce fotoğrafı (kare)",
        ["AfterSquarePath"] = "Sonra fotoğrafı (kare)",
        ["DurationWeeks"] = "Süre (hafta)",
        ["Year"] = "Yıl",
        ["AlignBottom"] = "Alta hizala",
        ["ShowOnHome"] = "Ana sayfada göster",
        ["Metrics"] = "Ölçümler",

        // Site içeriği
        ["Question"] = "Soru",
        ["Answer"] = "Cevap",
        ["Group"] = "Grup",
        ["Suffix"] = "Son ek",
        ["Platform"] = "Platform",
        ["Caption"] = "Açıklama",
        ["Hashtag"] = "Hashtag",
        ["PostedAt"] = "Paylaşım tarihi",
        ["HighlightText"] = "Vurgulu metin",
        ["BackgroundImagePath"] = "Arka plan görseli",
        ["ButtonUrl"] = "Buton bağlantısı",
        ["LegalEmail"] = "Yasal e-posta",
        ["Address"] = "Adres",
        ["WorkingDays1"] = "Çalışma günleri (1)",
        ["WorkingDays2"] = "Çalışma günleri (2)",
        ["WorkingHours1"] = "Çalışma saatleri (1)",
        ["WorkingHours2"] = "Çalışma saatleri (2)",
        ["DietitianFullName"] = "Diyetisyen adı",
        ["DietitianTitle"] = "Diyetisyen unvanı",
        ["DietitianPhotoPath"] = "Diyetisyen fotoğrafı",
        ["RecommendedPackageId"] = "Önerilen paket",

        // Sertifikalar
        ["Issuer"] = "Kurum (kısa ad)",
        ["IssuerFullName"] = "Kurumun tam adı",
        ["Detail"] = "Ek bilgi",
        ["LogoPath"] = "Logo",

        // Satın alma adımları
        ["TimeLabel"] = "Süre etiketi",

        // Koç bölümü (profil)
        ["CoachExperience"] = "Deneyim",
        ["CoachStory"] = "Koç hikâyesi",
        ["CoachVideoUrl"] = "Tanıtım videosu bağlantısı",
        ["CoachVideoCoverPath"] = "Video kapak görseli",

        // Formlar
        ["Gender"] = "Cinsiyet",
        ["Age"] = "Yaş",
        ["HeightCm"] = "Boy",
        ["CurrentWeight"] = "Mevcut kilo",
        ["TargetWeight"] = "Hedef kilo",
        ["ClientFormUrl"] = "Google Form linki",
        ["PrivacyAccepted"] = "Gizlilik onayı",
        ["HealthDataConsent"] = "Sağlık verisi onayı",
        ["Consents"] = "Onaylar"
    };

    public static string? Find(string name) => Names.GetValueOrDefault(name);

    public static string Get(string name) => Find(name) ?? name;
}
