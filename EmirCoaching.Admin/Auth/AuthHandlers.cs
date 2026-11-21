using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace EmirCoaching.Admin.Auth;

// Tarayıcı fetch'i cookie'yi başka porttaki/alt alan adındaki API'ye ancak "include" ile gönderir (refresh_token)
public class IncludeCredentialsHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        return base.SendAsync(request, cancellationToken);
    }
}

// Panelin tüm API istekleri: token'ı ekler, süresi dolmak üzereyse önce yeniler; 401 gelirse oturumu kapatır
public class BearerTokenHandler(AuthSession session, Lazy<AuthService> auth) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (session.IsAuthenticated && session.IsExpiring)
            await auth.Value.RefreshAsync();

        if (session.AccessToken is { } token)
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            auth.Value.ExpireSession();

        return response;
    }
}

// Yalnızca AuthService'in kullandığı istemci (Bearer handler'ı yok)
public class AuthHttpClient(HttpClient client)
{
    public HttpClient Client { get; } = client;
}
