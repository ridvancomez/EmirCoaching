namespace EmirCoaching.Business.Common;

// Koç hikâyesi panelde düz metin girilir; boş satır paragraf ayırır (sitede her paragraf ayrı <p>)
public static class CoachStory
{
    public static List<string> Paragraphs(string? story) =>
        string.IsNullOrWhiteSpace(story)
            ? []
            : story.Replace("\r\n", "\n")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();
}
