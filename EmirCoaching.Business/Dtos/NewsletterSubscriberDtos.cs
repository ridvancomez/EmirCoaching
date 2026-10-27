using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

// Footer bülten formu. Tek onay kutusu Gizlilik + Ticari İleti'yi birlikte kapsar:
// { "email": "...", "consents": { "Gizlilik": true, "TicariIleti": true } }
public record SubscribeNewsletterRequest
{
    public string Email { get; init; } = string.Empty;
    public Dictionary<ConsentType, bool> Consents { get; init; } = [];
}

public record NewsletterSubscriberResponse
{
    public int Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
}
