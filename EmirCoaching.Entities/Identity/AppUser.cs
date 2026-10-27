using EmirCoaching.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace EmirCoaching.Entities.Identity;

/// <summary>
/// Admin paneline giriş yapan kullanıcı. Blog yazarı da bu tablodan gelir
/// (blog detaydaki yazar kartı: ad, unvan, biyografi, fotoğraf).
/// </summary>
public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; } // "Kurucu & Baş Antrenör"
    public string? Bio { get; set; }
    public string? AvatarPath { get; set; }

    public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
