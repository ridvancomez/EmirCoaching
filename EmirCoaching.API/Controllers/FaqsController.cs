using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/faqs")]
public class FaqsController(IFaqService faqService) : ApiControllerBase
{
    // Sayfaya göre SSS: ?group=Paket (Genel | Paket | Diyet)
    [HttpGet]
    public async Task<IActionResult> GetByGroup(FaqGroup group) => FromResult(await faqService.GetByGroupAsync(group));
}
