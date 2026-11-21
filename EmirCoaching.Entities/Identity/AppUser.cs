using EmirCoaching.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace EmirCoaching.Entities.Identity;

/// <summary>
/// Admin paneline giriş yapan kullanıcı. Blog yazarı da bu tablodan gelir
/// (blog detaydaki yazar kartı: ad, unvan, biyografi, fotoğraf).
/// IsCoach işaretli tek kullanıcı ana sayfa ve Hakkımda'daki koç bölümünde gösterilir (ad, unvan ve fotoğraf
/// ortak; Coach* alanları yalnızca o bölüm için). Blog yazar kartında unvan yalnızca koçta görünür.
/// </summary>
public class AppUser : IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; } // "Baş Antrenör"
    public string? Bio { get; set; }
    public string? AvatarPath { get; set; }

    // Koç bölümü (yalnızca IsCoach işaretli kullanıcıda dolu)
    public bool IsCoach { get; set; }
    public string? CoachExperience { get; set; }     // "8+ yıl"
    public string? CoachStory { get; set; }          // paragraflar boş satırla ayrılır
    public string? CoachVideoUrl { get; set; }       // tanıtım videosu (data-video-src)
    public string? CoachVideoCoverPath { get; set; } // video kapak görseli

    public ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
}
