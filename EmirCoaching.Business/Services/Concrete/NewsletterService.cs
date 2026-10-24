using EmirCoaching.Business.Common;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.Business.Services.Concrete;

public class NewsletterService(IUnitOfWork unitOfWork, IConsentService consentService) : INewsletterService
{
    private static readonly NewsletterEmailValidator EmailValidator = new();

    public async Task<Result> SubscribeAsync(string email, Dictionary<ConsentType, bool> consents, RequestInfo requestInfo)
    {
        email = (email ?? string.Empty).Trim().ToLowerInvariant();

        var errors = await EmailValidator.GetErrorsAsync(email);
        if (!consents.TryGetValue(ConsentType.Gizlilik, out var privacyAccepted) || !privacyAccepted)
            errors.Add("Gizlilik Politikası onayı zorunludur.");
        if (consents.Keys.Any(type => !Enum.IsDefined(type)))
            errors.Add("Geçersiz onay türü.");
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var subscriber = await unitOfWork.NewsletterSubscribers.GetByEmailAsync(email);
        if (subscriber is null)
        {
            subscriber = new NewsletterSubscriber { Email = email, IsActive = true };
            await unitOfWork.NewsletterSubscribers.AddAsync(subscriber);
        }
        else
        {
            subscriber.IsActive = true;
        }

        // Her başvuruda onaylar yeniden kaydedilir (tarih + IP + metin sürümü)
        foreach (var (type, granted) in consents)
        {
            await consentService.RecordAsync(new ConsentRecord
            {
                ConsentType = type,
                IsGranted = granted,
                IpAddress = requestInfo.IpAddress,
                UserAgent = requestInfo.UserAgent,
                NewsletterSubscriber = subscriber
            });
        }

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
