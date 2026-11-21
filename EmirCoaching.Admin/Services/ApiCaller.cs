using EmirCoaching.Admin.Api;
using MudBlazor;

namespace EmirCoaching.Admin.Services;

// Sayfalardaki API çağrılarını sarar: hata olursa mesajı sağ altta gösterir, sayfa try/catch yazmaz
public class ApiCaller(ISnackbar snackbar)
{
    // Veri okuma: hatada null
    public async Task<T?> GetAsync<T>(Func<Task<T>> call) where T : class
    {
        try
        {
            return await call();
        }
        catch (ApiException ex)
        {
            ShowError(ex);
            return null;
        }
        catch (HttpRequestException)
        {
            ShowError("API'ye ulaşılamadı. Bağlantıyı kontrol edin.");
            return null;
        }
    }

    // Olmayabilecek kayıt (ör. henüz yayınlanmamış yasal metin): 404'te hata göstermeden null
    public async Task<T?> GetOptionalAsync<T>(Func<Task<T>> call) where T : class
    {
        try
        {
            return await call();
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return null;
        }
        catch (ApiException ex)
        {
            ShowError(ex);
            return null;
        }
        catch (HttpRequestException)
        {
            ShowError("API'ye ulaşılamadı. Bağlantıyı kontrol edin.");
            return null;
        }
    }

    // Yazma işlemi: başarıda true (+ isteğe bağlı bilgi mesajı)
    public async Task<bool> RunAsync(Func<Task> call, string? successMessage = null)
    {
        try
        {
            await call();
            if (successMessage is not null)
                snackbar.Add(successMessage, Severity.Success);
            return true;
        }
        catch (ApiException ex)
        {
            ShowError(ex);
            return false;
        }
        catch (HttpRequestException)
        {
            ShowError("API'ye ulaşılamadı. Bağlantıyı kontrol edin.");
            return false;
        }
    }

    public void ShowError(ApiException ex)
    {
        // 401'de BearerTokenHandler zaten giriş ekranına yönlendirir
        if (ex.StatusCode != 401)
            ShowError(ApiErrors.Describe(ex));
    }

    public void ShowError(string message) => snackbar.Add(message, Severity.Error);

    public void ShowSuccess(string message) => snackbar.Add(message, Severity.Success);
}
