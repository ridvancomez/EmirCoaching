namespace EmirCoaching.API.Common;

// [EnableRateLimiting(...)] ile kullanılan politika adları (tanımlar Program.cs'te)
public static class RateLimitPolicies
{
    // Herkese açık formlar: IP başına dakikada 5 istek
    public const string Forms = "forms";

    // Admin girişi: IP başına dakikada 5 deneme (hesap kilidine ek olarak)
    public const string Login = "login";
}
