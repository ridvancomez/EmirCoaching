namespace EmirCoaching.Entities.Entities;

/// <summary>
/// Blog yazısı ile etiket arasındaki bağlantı tablosu. BaseEntity'den türemez:
/// bağlantı kaldırılınca kayıt gerçekten silinir.
/// </summary>
public class BlogPostTag
{
    public int BlogPostId { get; set; }
    public BlogPost BlogPost { get; set; } = null!;

    public int BlogTagId { get; set; }
    public BlogTag BlogTag { get; set; } = null!;
}
