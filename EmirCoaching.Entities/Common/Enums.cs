namespace EmirCoaching.Entities.Common;

// Tüm enum'lar veritabanında int olarak saklanır; mevcut değerler değiştirilmez.

/// <summary>Paketin antrenman yeri; paketler sayfasındaki kategori filtresi buna göre çalışır.</summary>
public enum PackageLocation
{
    Salonda = 0,
    Evde = 1 // front-end'de body.tema-evde
}

/// <summary>Değişimler sayfasında kartın hangi bölümde gösterileceği (admin seçer).</summary>
public enum TransformationSection
{
    Carousel = 0, // .degisim-carousel ("Tüm Değişimler")
    Galeri = 1    // .degisim-galeri ("Önce ve sonra galerisi")
}

/// <summary>SSS kayıtlarının gösterildiği yer.</summary>
public enum FaqGroup
{
    Genel = 0,
    Paket = 1,
    Diyet = 2
}

/// <summary>Yasal metin türleri; her türün aynı anda tek güncel sürümü olur.</summary>
public enum LegalDocumentType
{
    Kvkk = 0,
    AcikRiza = 1,
    Cerez = 2,
    IptalIade = 3,
    TicariIleti = 4,
    Gizlilik = 5
}

/// <summary>"Bize Yazın" mesajlarının admin panelindeki takip durumu.</summary>
public enum ContactMessageStatus
{
    Yeni = 0,
    Okundu = 1,
    Donuldu = 2
}

/// <summary>Diyet formu başvurularının admin panelindeki takip durumu.</summary>
public enum DietFormStatus
{
    Yeni = 0,
    Okundu = 1,
    Donuldu = 2
}

/// <summary>Diyet formundaki günlük aktivite seviyesi (formdaki 5 seçenek).</summary>
public enum ActivityLevel
{
    Sedentary = 0,  // Hareketsiz (masa başı)
    Light = 1,      // Az aktif
    Moderate = 2,   // Orta derece aktif
    Active = 3,     // Aktif
    VeryActive = 4  // Çok aktif
}

/// <summary>Diyet formundaki cinsiyet seçimi.</summary>
public enum Gender
{
    Erkek = 0,
    Kadin = 1
}

/// <summary>Kullanıcıdan alınan onayın türü (KVKK / İYS kaydı için).</summary>
public enum ConsentType
{
    Gizlilik = 0,
    AcikRizaSaglik = 1,
    TicariIleti = 2
}

/// <summary>Sosyal medya platformları; iletişim kartları ve ana sayfa postlarında kullanılır.</summary>
public enum SocialPlatform
{
    WhatsApp = 0,
    Instagram = 1,
    YouTube = 2,
    Facebook = 3
}

/// <summary>
/// "Nasıl Çalışır?" adımının ikonu; admin yalnızca bu üçünden seçer. Değerler sitedeki adım sırasıyla aynıdır.
/// İkonun adı (Iconify) Business/Common/DietProgramStepIcons.cs'te.
/// </summary>
public enum DietProgramStepIcon
{
    FormuDoldur = 1,    // mdi:clipboard-text
    PlanHazirlanir = 2, // mdi:silverware-fork-knife
    WhatsApp = 3        // mdi:message-text
}

/// <summary>Admin giriş denemesinin sonucu (giriş geçmişi).</summary>
public enum LoginAttemptResult
{
    Basarili = 0,
    KullaniciYok = 1,
    HataliSifre = 2,
    Kilitli = 3
}
