using EmirCoaching.API.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers;

[Route("api/v1/purchase-steps")]
public class PurchaseStepsController(IPurchaseStepService purchaseStepService) : ApiControllerBase
{
    // Ana sayfadaki "Satın aldıktan sonra ne oluyor?" adımları, sıralı → 200
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PurchaseStepResponse>>> GetAll() => FromResult(await purchaseStepService.GetAllAsync());
}
