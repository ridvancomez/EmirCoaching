using EmirCoaching.API.Common;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Entities.Common;
using Microsoft.AspNetCore.Mvc;

namespace EmirCoaching.API.Controllers.Admin;

// Giriş geçmişi (90 gün saklanır); denemeler AuthService'te kaydedilir
[Route("api/v1/admin/login-attempts")]
public class LoginAttemptsController(ILoginAttemptService loginAttemptService) : AdminControllerBase
{
    private const int DefaultPageSize = 20;

    // Yeniden eskiye, sonuca göre filtre: ?result=HataliSifre&page=1&size=20 → 200
    [HttpGet]
    public async Task<IActionResult> GetAll(LoginAttemptResult? result, int page = 1, int size = DefaultPageSize) =>
        FromResult(await loginAttemptService.GetPagedAsync(result, page, size));
}
