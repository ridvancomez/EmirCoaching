using EmirCoaching.Admin.Api;

namespace EmirCoaching.Admin.Services;

// Enum'ların panelde görünen Türkçe adları
public static class Labels
{
    public static string Of(FaqGroup value) => value switch
    {
        FaqGroup.Genel => "Genel",
        FaqGroup.Paket => "Paket",
        FaqGroup.Diyet => "Diyet",
        _ => value.ToString()
    };

    public static string Of(PackageLocation value) => value switch
    {
        PackageLocation.Salonda => "Salonda",
        PackageLocation.Evde => "Evde",
        _ => value.ToString()
    };

    public static string Of(SocialPlatform value) => value switch
    {
        SocialPlatform.WhatsApp => "WhatsApp",
        SocialPlatform.Instagram => "Instagram",
        SocialPlatform.YouTube => "YouTube",
        SocialPlatform.Facebook => "Facebook",
        _ => value.ToString()
    };

    public static string Of(DietFormStatus value) => value switch
    {
        DietFormStatus.Yeni => "Yeni",
        DietFormStatus.Okundu => "Okundu",
        DietFormStatus.Donuldu => "Dönüldü",
        _ => value.ToString()
    };

    public static string Of(ContactMessageStatus value) => value switch
    {
        ContactMessageStatus.Yeni => "Yeni",
        ContactMessageStatus.Okundu => "Okundu",
        ContactMessageStatus.Donuldu => "Dönüldü",
        _ => value.ToString()
    };

    public static string Of(Gender value) => value switch
    {
        Gender.Erkek => "Erkek",
        Gender.Kadin => "Kadın",
        _ => value.ToString()
    };

    public static string Of(DietProgramStepIcon value) => value switch
    {
        DietProgramStepIcon.FormuDoldur => "Formu doldur",
        DietProgramStepIcon.PlanHazirlanir => "Plan hazırlanır",
        DietProgramStepIcon.WhatsApp => "WhatsApp",
        _ => value.ToString()
    };

    public static string Of(TransformationSection value) => value switch
    {
        TransformationSection.Carousel => "Kaydırmalı (üst)",
        TransformationSection.Galeri => "Galeri",
        _ => value.ToString()
    };

    public static string Of(LoginAttemptResult value) => value switch
    {
        LoginAttemptResult.Basarili => "Başarılı",
        LoginAttemptResult.KullaniciYok => "Kullanıcı yok",
        LoginAttemptResult.HataliSifre => "Hatalı şifre",
        LoginAttemptResult.Kilitli => "Kilitli",
        _ => value.ToString()
    };

    public static string Of(LegalDocumentType value) => value switch
    {
        LegalDocumentType.Kvkk => "KVKK Aydınlatma Metni",
        LegalDocumentType.AcikRiza => "Açık Rıza Metni",
        LegalDocumentType.Cerez => "Çerez Politikası",
        LegalDocumentType.IptalIade => "İptal ve İade Koşulları",
        LegalDocumentType.TicariIleti => "Ticari Elektronik İleti Onayı",
        LegalDocumentType.Gizlilik => "Gizlilik Politikası",
        LegalDocumentType.MesafeliSatis => "Mesafeli Satış Sözleşmesi",
        _ => value.ToString()
    };

    public static string Date(DateTime value) => value.ToLocalTime().ToString("dd.MM.yyyy HH:mm");

    public static string Price(double value) => "₺ " + value.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("tr-TR"));
}
