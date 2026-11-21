using System.Net.Http.Headers;
using EmirCoaching.Admin.Api;
using EmirCoaching.Admin.Services;
using Microsoft.AspNetCore.Components;

namespace EmirCoaching.Admin.Auth;

// Giriş, çıkış ve token yenileme. Kendi HttpClient'ı vardır (AuthHandler'sız): yenileme isteği kendini tetiklemesin.
public class AuthService(AuthSession session, NavigationManager navigation, AuthHttpClient authHttp)
{
    private readonly AuthClient _client = new(authHttp.Client);

    // Refresh token her kullanımda değişir; aynı anda iki yenileme eskisini iki kez kullanır ve API tüm
    // oturumları kapatır. Bu yüzden yenileme tek sıradan geçer.
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    // Başarıda null, hatada kullanıcıya gösterilecek mesaj
    public async Task<string?> LoginAsync(string email, string password)
    {
        try
        {
            var tokens = await _client.LoginAsync(new LoginRequest { Email = email, Password = password });
            SetTokens(tokens);
            await LoadUserAsync();
            return null;
        }
        catch (ApiException ex)
        {
            return ApiErrors.Describe(ex);
        }
    }

    // Sayfa açılışında: cookie'deki refresh token geçerliyse oturum sessizce geri gelir
    public async Task<bool> TryRestoreAsync() => session.IsAuthenticated || await RefreshAsync();

    public async Task<bool> RefreshAsync()
    {
        await _refreshLock.WaitAsync();
        try
        {
            // Beklerken başka bir istek yenilediyse tekrar yenilenmez
            if (session.IsAuthenticated && !session.IsExpiring)
                return true;

            SetTokens(await _client.RefreshAsync());
            if (session.User is null)
                await LoadUserAsync();
            return true;
        }
        catch (ApiException)
        {
            session.Clear();
            return false;
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    public async Task LoadUserAsync() => session.SetUser(await _client.MeAsync());

    // Şifre değişince API diğer oturumları kapatır ve bu oturuma yeni token verir
    public void SetTokens(AuthResponse tokens)
    {
        session.SetTokens(tokens);
        authHttp.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _client.LogoutAsync();
        }
        catch (ApiException)
        {
            // Cookie zaten geçersizse de çıkış yapılmış sayılır
        }

        session.Clear();
        authHttp.Client.DefaultRequestHeaders.Authorization = null;
        navigation.NavigateTo("/login");
    }

    // Yenilenemeyen oturum: giriş ekranına, döndükten sonra aynı sayfaya geri gelinir
    public void ExpireSession()
    {
        if (!session.IsAuthenticated)
            return;

        session.Clear();
        var returnUrl = Uri.EscapeDataString(navigation.ToBaseRelativePath(navigation.Uri));
        navigation.NavigateTo($"/login?expired=true&returnUrl={returnUrl}");
    }
}
