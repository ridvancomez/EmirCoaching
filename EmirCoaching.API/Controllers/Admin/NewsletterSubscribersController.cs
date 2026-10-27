using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

[Route("api/v1/admin/newsletter-subscribers")]
public class NewsletterSubscribersController(INewsletterSubscriberService newsletterSubscriberService) : AdminControllerBase
{
    private const int DefaultPageSize = 20;

    // Yeniden eskiye: ?page=1&size=20 → 200
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = 1, int size = DefaultPageSize) =>
        FromResult(await newsletterSubscriberService.GetPagedAsync(page, size));

    // KVKK: kayıt onay kayıtlarıyla birlikte kalıcı silinir → 204 / 404
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id) =>
        FromResult(await newsletterSubscriberService.DeleteAsync(id));
}
