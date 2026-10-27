using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/contact-messages")]
[EnableRateLimiting(RateLimitPolicies.Forms)]
public class ContactMessagesController(IContactMessageService contactMessageService) : ApiControllerBase
{
    // İletişim modalı formu → 204
    [HttpPost]
    public async Task<IActionResult> Submit(CreateContactMessageRequest request) =>
        FromResult(await contactMessageService.SubmitAsync(request, GetRequestInfo()));
}
