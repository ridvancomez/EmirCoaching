using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class BlogCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
