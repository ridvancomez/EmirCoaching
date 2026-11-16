using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace EmirCoaching.API.Common;

// Panelin istemcisi (NSwag) bu belgeden üretilir; belge kodun gerçek davranışını yansıtsın diye düzeltmeler
public sealed class OpenApiClientTransformer : IOpenApiOperationTransformer, IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        if (context.Description.ActionDescriptor is not ControllerActionDescriptor action)
            return Task.CompletedTask;

        // İstemcide sınıf + metot adı olur (AdminSiteStats_GetAll → AdminSiteStatsClient.GetAllAsync);
        // site ve admin controller'larının adı aynı olabildiği için area başa eklenir
        var area = action.RouteValues.TryGetValue("area", out var value) ? value : null;
        operation.OperationId = $"{area}{action.ControllerName}_{action.ActionName}";

        var returnType = action.MethodInfo.ReturnType;

        // Veri dönmeyen uçlar (Task<IActionResult>, FromResult(Result)) 204 döner; ASP.NET bunu bilemediği için 200 yazar
        if (returnType == typeof(Task<IActionResult>))
            operation.Responses = new OpenApiResponses { ["204"] = new OpenApiResponse { Description = "No Content" } };

        // CreatedFromResult 201 döner
        else if (returnType == typeof(Task<ActionResult<CreatedResponse>>)
                 && operation.Responses?.Remove("200", out var created) == true)
            operation.Responses["201"] = created;

        return Task.CompletedTask;
    }

    // API "5" gibi tırnaklı sayıyı da okur (web varsayılanı), belge bu yüzden "integer | string" yazar;
    // istemci her zaman sayı gönderdiği için belgede yalnızca sayı kalır
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (schema.Type is { } type
            && type.HasFlag(JsonSchemaType.String)
            && (type.HasFlag(JsonSchemaType.Integer) || type.HasFlag(JsonSchemaType.Number)))
        {
            schema.Type = type & ~JsonSchemaType.String;
            schema.Pattern = null;
        }

        // Enum'lar sayı olarak gider ama belge yalnızca "integer" der; değerler ve adları (x-enumNames, NSwag okur)
        // eklenmezse istemcide PackageLocation yerine çıplak int olur
        var enumType = Nullable.GetUnderlyingType(context.JsonTypeInfo.Type) ?? context.JsonTypeInfo.Type;
        if (enumType.IsEnum)
        {
            var names = Enum.GetNames(enumType);
            schema.Enum = [.. names.Select(name => JsonValue.Create(Convert.ToInt32(Enum.Parse(enumType, name))))];
            schema.Extensions ??= new Dictionary<string, IOpenApiExtension>();
            schema.Extensions["x-enumNames"] = new JsonNodeExtension(new JsonArray([.. names.Select(name => JsonValue.Create(name))]));
        }

        return Task.CompletedTask;
    }
}
