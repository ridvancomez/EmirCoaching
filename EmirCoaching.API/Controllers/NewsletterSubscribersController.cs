using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/newsletter-subscribers")]
[EnableRateLimiting(RateLimitPolicies.Forms)]
public class NewsletterSubscribersController(INewsletterSubscriberService newsletterSubscriberService) : ApiControllerBase
{
    // Footer bülten formu → 204 (aynı e-posta tekrar gelirse abonelik yeniden aktifleşir)
    [HttpPost]
    public async Task<IActionResult> Subscribe(SubscribeNewsletterRequest request) =>
        FromResult(await newsletterSubscriberService.SubscribeAsync(request, GetRequestInfo()));
}
