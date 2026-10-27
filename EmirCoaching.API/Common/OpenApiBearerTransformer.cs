using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace EmirCoaching.API.Common;

// OpenAPI belgesine JWT şemasını ekler; [Authorize] uçları (admin, /auth/me) Swagger'da kilitli görünür
// ve "Authorize" butonuna yapıştırılan token ile çağrılır.
public sealed class OpenApiBearerTransformer : IOpenApiDocumentTransformer, IOpenApiOperationTransformer
{
    private const string SchemeName = "Bearer";

    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info.Title = "EmirCoaching API";
        document.Info.Description = "Site ve admin paneli uçları. Admin uçları için önce POST /api/v1/auth/login, " +
                                    "dönen accessToken'ı \"Authorize\" butonuna yapıştırın.";

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes[SchemeName] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Giriş yanıtındaki accessToken (15 dk geçerli)"
        };

        return Task.CompletedTask;
    }

    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        var metadata = context.Description.ActionDescriptor.EndpointMetadata;
        if (metadata.OfType<IAuthorizeData>().Any() && !metadata.OfType<IAllowAnonymous>().Any())
        {
            operation.Security =
            [
                new OpenApiSecurityRequirement { [new OpenApiSecuritySchemeReference(SchemeName, context.Document)] = [] }
            ];
        }

        return Task.CompletedTask;
    }
}
