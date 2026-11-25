namespace EmirCoaching.Web.Services;

/// <summary>
/// Üst menü ve mobil menüde sayfanın aktif öğesi. View, ViewData["ActiveNav"] ile anahtarı verir
/// (anasayfa, paketler, hakkimda, degisimler, iletisim, blog).
/// </summary>
public sealed class NavState(string? activeKey)
{
    /// <summary>Öğe aktifse verilen sınıfı (baştaki boşlukla), değilse boş metni döner.</summary>
    public string Active(string key, string cssClass) => key == activeKey ? cssClass : string.Empty;

    /// <summary>aria-current değeri; null dönünce Razor özniteliği hiç basmaz.</summary>
    public string? Current(string key) => key == activeKey ? "page" : null;
}
