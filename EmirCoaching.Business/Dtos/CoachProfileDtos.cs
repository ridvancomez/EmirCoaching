namespace EmirCoaching.Business.Dtos;

// Ana sayfa ve Hakkımda'daki koç bölümü; IsCoach işaretli kullanıcıdan gelir (panelde profil ayarlarından düzenlenir)
public record CoachProfileResponse
{
    public string FullName { get; init; } = string.Empty;
    public string? Title { get; init; }       // "Online Fitness & Beslenme Koçu"
    public string? Experience { get; init; }  // "8+ yıl"
    public IReadOnlyList<string> StoryParagraphs { get; init; } = [];
    public string? AvatarUrl { get; init; }
    public string? VideoUrl { get; init; }
    public string? VideoCoverUrl { get; init; }
}
