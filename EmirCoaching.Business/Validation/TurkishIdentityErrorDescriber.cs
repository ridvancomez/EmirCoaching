using Microsoft.AspNetCore.Identity;

namespace EmirCoaching.Business.Validation;

// ASP.NET Identity hata mesajları (şifre kuralları, kullanıcı oluşturma) Türkçe
public class TurkishIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() => Error(nameof(DefaultError), "Bilinmeyen bir hata oluştu.");
    public override IdentityError ConcurrencyFailure() => Error(nameof(ConcurrencyFailure), "Kayıt başka biri tarafından değiştirildi. Sayfayı yenileyip tekrar deneyin.");
    public override IdentityError PasswordMismatch() => Error(nameof(PasswordMismatch), "Şifre hatalı.");
    public override IdentityError InvalidToken() => Error(nameof(InvalidToken), "Geçersiz doğrulama kodu.");
    public override IdentityError RecoveryCodeRedemptionFailed() => Error(nameof(RecoveryCodeRedemptionFailed), "Kurtarma kodu kullanılamadı.");
    public override IdentityError LoginAlreadyAssociated() => Error(nameof(LoginAlreadyAssociated), "Bu giriş yöntemi başka bir kullanıcıya bağlı.");
    public override IdentityError InvalidUserName(string? userName) => Error(nameof(InvalidUserName), $"'{userName}' kullanıcı adı geçersiz.");
    public override IdentityError InvalidEmail(string? email) => Error(nameof(InvalidEmail), $"'{email}' geçerli bir e-posta adresi değil.");
    public override IdentityError DuplicateUserName(string userName) => Error(nameof(DuplicateUserName), $"'{userName}' kullanıcı adı zaten kullanılıyor.");
    public override IdentityError DuplicateEmail(string email) => Error(nameof(DuplicateEmail), $"'{email}' e-posta adresi zaten kullanılıyor.");
    public override IdentityError InvalidRoleName(string? role) => Error(nameof(InvalidRoleName), $"'{role}' rol adı geçersiz.");
    public override IdentityError DuplicateRoleName(string role) => Error(nameof(DuplicateRoleName), $"'{role}' rolü zaten var.");
    public override IdentityError UserAlreadyHasPassword() => Error(nameof(UserAlreadyHasPassword), "Kullanıcının zaten bir şifresi var.");
    public override IdentityError UserLockoutNotEnabled() => Error(nameof(UserLockoutNotEnabled), "Bu kullanıcı için hesap kilitleme açık değil.");
    public override IdentityError UserAlreadyInRole(string role) => Error(nameof(UserAlreadyInRole), $"Kullanıcı zaten '{role}' rolünde.");
    public override IdentityError UserNotInRole(string role) => Error(nameof(UserNotInRole), $"Kullanıcı '{role}' rolünde değil.");
    public override IdentityError PasswordTooShort(int length) => Error(nameof(PasswordTooShort), $"Şifre en az {length} karakter olmalı.");
    public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) => Error(nameof(PasswordRequiresUniqueChars), $"Şifrede en az {uniqueChars} farklı karakter olmalı.");
    public override IdentityError PasswordRequiresNonAlphanumeric() => Error(nameof(PasswordRequiresNonAlphanumeric), "Şifrede en az bir özel karakter (ör. ! * .) olmalı.");
    public override IdentityError PasswordRequiresDigit() => Error(nameof(PasswordRequiresDigit), "Şifrede en az bir rakam olmalı.");
    public override IdentityError PasswordRequiresLower() => Error(nameof(PasswordRequiresLower), "Şifrede en az bir küçük harf olmalı.");
    public override IdentityError PasswordRequiresUpper() => Error(nameof(PasswordRequiresUpper), "Şifrede en az bir büyük harf olmalı.");

    private static IdentityError Error(string code, string description) => new() { Code = code, Description = description };
}
