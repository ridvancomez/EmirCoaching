using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

// Paket detayındaki yorumlar paketle birlikte gelir (GET /packages/{slug}); burada yalnızca ana sayfa carousel'i
[Route("api/v1/testimonials")]
public class TestimonialsController(ITestimonialService testimonialService) : ApiControllerBase
{
    // ShowOnHome işaretli aktif yorumlar, paket adı ve süresiyle → 200
    [HttpGet("home")]
    public async Task<ActionResult<IReadOnlyList<TestimonialResponse>>> GetHome() =>
        FromResult(await testimonialService.GetHomeAsync());
}
