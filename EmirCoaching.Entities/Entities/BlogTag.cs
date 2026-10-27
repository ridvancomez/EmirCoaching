using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Blog etiketi; blog detayın sonunda "#beslenme", "#kalori-acigi" şeklinde listelenir.
/// Bir etiket birden çok yazıda kullanılabilir (BlogPostTag ile çoka-çok).
/// </summary>
public class BlogTag : BaseEntity, ISluggable
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<BlogPostTag> Posts { get; set; } = new List<BlogPostTag>();
}
