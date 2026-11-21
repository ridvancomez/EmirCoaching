using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Diyet planı detay sayfasındaki program ve onu hazırlayan diyetisyen (tek kayıt).
/// Diyet planı ayrı satılmaz, tüm paketlere dahildir: sağdaki kart paket listesini gösterir (ücret yok).
/// Diyetisyen admin kullanıcısı değildir; bilgileri burada tutulur. "Program İçeriği" maddeleri HTML'de sabittir.
/// </summary>
public class DietProgram : BaseEntity, IActivatable
{
    public string Title { get; set; } = string.Empty;

    // Diyetisyen kartı
    public string DietitianTitle { get; set; } = string.Empty;    // "Uzm. Dr."
    public string DietitianFullName { get; set; } = string.Empty; // "Ceyda Güler"
    public string? DietitianPhotoPath { get; set; }

    public bool IsActive { get; set; }
}
