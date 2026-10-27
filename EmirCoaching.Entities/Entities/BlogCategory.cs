using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Blog kategorisi (Beslenme, Hipertrofi, Kuvvet, Toparlanma, Mentalite).
/// Slug, blog sayfasındaki filtre adresinde kullanılır: blog.html?kategori={Slug}.
/// </summary>
public class BlogCategory : BaseEntity, ISluggable
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
