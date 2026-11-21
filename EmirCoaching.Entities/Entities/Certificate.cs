using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Koçun sertifikası; ana sayfadaki koç bölümünde ve Hakkımda'daki "Eğitim ve Sertifikalar" listesinde aynı kartlar.
/// Kartta logo, başlık ve "Kurum · ek bilgi · yıl" satırı ("FMI · EREPS Seviye 4 · 2022") bulunur.
/// </summary>
public class Certificate : BaseEntity, ISortable, IActivatable
{
    public string Title { get; set; } = string.Empty;          // "Personal Trainer EQF Level 4"
    public string Issuer { get; set; } = string.Empty;         // kısa ad: "FMI"
    public string IssuerFullName { get; set; } = string.Empty; // logonun alt/title metni: "Fitness Movement Institute"
    public string? Detail { get; set; }                        // "EREPS Seviye 4"
    public int Year { get; set; }
    public string LogoPath { get; set; } = string.Empty;

    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
