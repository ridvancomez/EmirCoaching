using System.Security.Claims;
using EmirCoaching.Business.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EmirCoaching.API.Common;

// Tüm controller'ların tabanı: servisten gelen Result'ı HTTP cevabına çevirir.
// Başarı → veri (zarfsız), hata → ProblemDetails (400 / 401 / 404 / 409 / 429).
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    // GET → 200 + veri
    protected IActionResult FromResult<T>(Result<T> result) =>
        result.IsSuccess ? Ok(result.Value) : Failure(result);

    // Başarıda yanıtı controller kurar (ör. cookie yazıp farklı bir DTO döner)
    protected IActionResult FromResult<T>(Result<T> result, Func<T, IActionResult> onSuccess) =>
        result.IsSuccess ? onSuccess(result.Value!) : Failure(result);

    // PUT / DELETE → 204
    protected IActionResult FromResult(Result result) =>
        result.IsSuccess ? NoContent() : Failure(result);

    // POST → 201 + Location (GET action'ı) + { id }
    protected IActionResult CreatedFromResult(Result<int> result, string getActionName) =>
        result.IsSuccess
            ? CreatedAtAction(getActionName, new { id = result.Value }, new { id = result.Value })
            : Failure(result);

    // Onay kayıtları (KVKK) için isteğin IP ve tarayıcı bilgisi
    protected RequestInfo GetRequestInfo() => new(
        HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
        Request.Headers.UserAgent.ToString());

    // [Authorize] uçlarında token'daki kullanıcı (sub claim'i)
    protected int CurrentUserId => int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)
        ?? throw new InvalidOperationException("Kullanıcı bilgisi yok; uç [Authorize] ile korunmalı."));

    private ObjectResult Failure(Result result)
    {
        var status = result.ErrorType switch
        {
            ResultErrorType.NotFound => StatusCodes.Status404NotFound,
            ResultErrorType.Conflict => StatusCodes.Status409Conflict,
            ResultErrorType.TooManyRequests => StatusCodes.Status429TooManyRequests,
            ResultErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status400BadRequest
        };

        return ApiProblem.Create(HttpContext, status, result.Errors);
    }
}
