using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Mapping;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class NewsletterSubscriberService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConsentRecordService consentRecordService,
    ILogger<NewsletterSubscriberService> logger) : INewsletterSubscriberService
{
    private const int MaxNewSubscriptionsPerIp = 3;
    private static readonly TimeSpan IpLimitWindow = TimeSpan.FromHours(24);
    private static readonly NewsletterEmailValidator EmailValidator = new();

    public async Task<Result> SubscribeAsync(SubscribeNewsletterRequest request, RequestInfo requestInfo)
    {
        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();
        var consents = request.Consents ?? [];

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
            // Aynı IP'den 24 saatte en fazla 3 yeni abonelik (var olan e-postanın yenilenmesi sayılmaz)
            var since = DateTime.UtcNow - IpLimitWindow;
            if (await unitOfWork.NewsletterSubscribers.CountByIpSinceAsync(requestInfo.IpAddress, since) >= MaxNewSubscriptionsPerIp)
            {
                logger.LogWarning("Bülten aboneliği reddedildi: IP günlük sınırı aşıldı");
                return Result.TooManyRequests("Bu bağlantıdan bugün çok fazla abonelik yapıldı. Lütfen daha sonra tekrar deneyin.");
            }

            subscriber = new NewsletterSubscriber { Email = email, IsActive = true, IpAddress = requestInfo.IpAddress };
            await unitOfWork.NewsletterSubscribers.AddAsync(subscriber);
        }
        else
        {
            subscriber.IsActive = true;
        }

        // Her başvuruda onaylar yeniden kaydedilir (tarih + IP + metin sürümü)
        foreach (var (type, granted) in consents)
        {
            await consentRecordService.RecordAsync(new ConsentRecord
            {
                ConsentType = type,
                IsGranted = granted,
                IpAddress = requestInfo.IpAddress,
                UserAgent = requestInfo.UserAgent,
                NewsletterSubscriber = subscriber
            });
        }

        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Bülten aboneliği {Id} kaydedildi", subscriber.Id);
        return Result.Success();
    }

    public async Task<Result<PagedResult<NewsletterSubscriberResponse>>> GetPagedAsync(int page, int size)
    {
        var subscribers = await unitOfWork.NewsletterSubscribers.GetPagedAsync(page, size);
        return Result<PagedResult<NewsletterSubscriberResponse>>.Success(
            mapper.MapPaged<NewsletterSubscriber, NewsletterSubscriberResponse>(subscribers));
    }

    // KVKK silme talebi: kayıt onaylarıyla birlikte kalıcı silinir
    public async Task<Result> DeleteAsync(int id)
    {
        if (!await unitOfWork.DeletePermanentlyAsync(unitOfWork.NewsletterSubscribers, id))
            return Result.NotFound("Abone bulunamadı.");

        logger.LogInformation("Bülten aboneliği {Id} kalıcı olarak silindi", id);
        return Result.Success();
    }
}
