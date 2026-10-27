using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/diet-form-submissions")]
[EnableRateLimiting(RateLimitPolicies.Forms)]
public class DietFormSubmissionsController(IDietFormSubmissionService dietFormSubmissionService) : ApiControllerBase
{
    // Diyet formu → 204
    [HttpPost]
    public async Task<IActionResult> Submit(CreateDietFormSubmissionRequest request) =>
        FromResult(await dietFormSubmissionService.SubmitAsync(request, GetRequestInfo()));
}
