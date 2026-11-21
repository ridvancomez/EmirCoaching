namespace EmirCoaching.Admin.Services;

// Sitenin tanıdığı ikon anahtarları (seed ve front-end ile aynı); panelde listeden seçilir
public static class IconKeys
{
    // Ana sayfa "Satın aldıktan sonra ne oluyor?"
    public static readonly IReadOnlyDictionary<string, string> PurchaseStep = new Dictionary<string, string>
    {
        ["form"] = "Form",
        ["saat"] = "Saat",
        ["takvim"] = "Takvim",
        ["revize"] = "Revize"
    };

    // Paket kartındaki öne çıkan maddeler
    public static readonly IReadOnlyDictionary<string, string> PackageHighlight = new Dictionary<string, string>
    {
        ["antrenman"] = "Antrenman",
        ["beslenme"] = "Beslenme",
        ["tahlil"] = "Tahlil",
        ["takvim"] = "Takvim",
        ["whatsapp"] = "WhatsApp",
        ["zoom"] = "Zoom"
    };

    // Paket detayındaki modüller
    public static readonly IReadOnlyDictionary<string, string> PackageModule = new Dictionary<string, string>
    {
        ["antrenman"] = "Antrenman",
        ["beslenme"] = "Beslenme",
        ["takip-olcum"] = "Takip & ölçüm",
        ["kocluk"] = "Koçluk"
    };
}
