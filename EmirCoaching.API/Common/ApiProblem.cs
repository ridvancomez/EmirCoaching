using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace EmirCoaching.API.Common;

// Tek hata formatı: ProblemDetails + "errors" (mesaj listesi).
// Servis hataları (ApiControllerBase) ve model binding hataları (Program.cs) aynı şekli kullanır.
public static class ApiProblem
{
    private const string ValidationTitle = "Gönderilen bilgiler geçersiz.";

    public static ObjectResult Create(HttpContext httpContext, int status, IReadOnlyList<string> errors)
    {
        // Doğrulamada birden çok mesaj olabilir; diğerlerinde başlık servisin mesajıdır
        var title = status == StatusCodes.Status400BadRequest || errors.Count == 0 ? ValidationTitle : errors[0];

        var factory = httpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
        var problem = factory.CreateProblemDetails(httpContext, status, title);
        problem.Extensions["errors"] = errors;

        return new ObjectResult(problem) { StatusCode = status, ContentTypes = { "application/problem+json" } };
    }

    // Controller dışından (istek sınırı, JWT 401/403) aynı formatta yanıt yazar
    public static Task WriteAsync(HttpContext httpContext, int status, IReadOnlyList<string> errors) =>
        Create(httpContext, status, errors).ExecuteResultAsync(
            new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor()));
}
