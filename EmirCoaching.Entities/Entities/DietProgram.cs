using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Diyet planı detay sayfasındaki program ve onu hazırlayan diyetisyen (şimdilik tek kayıt).
/// Diyetisyen admin kullanıcısı değildir; bilgileri burada tutulur. "Program İçeriği" maddeleri
/// şimdilik HTML'de sabittir. Ücret kartındaki buton diyet formuna gider.
/// </summary>
public class DietProgram : BaseEntity, IActivatable
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }       // ₺ 1.950
    public int DurationWeeks { get; set; }   // "4 haftalık program"

    // Diyetisyen kartı
    public string DietitianTitle { get; set; } = string.Empty;    // "Uzm. Dr."
    public string DietitianFullName { get; set; } = string.Empty; // "Ceyda Güler"
    public string? DietitianPhotoPath { get; set; }

    public bool IsActive { get; set; }
}
