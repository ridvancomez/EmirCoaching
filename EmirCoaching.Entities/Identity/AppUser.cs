using EmirCoaching.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace EmirCoaching.Entities.Identity;

public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string? Bio { get; set; }
    public string? AvatarPath { get; set; }

    public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
