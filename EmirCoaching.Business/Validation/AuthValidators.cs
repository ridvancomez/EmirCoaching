using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Password).NotEmpty().MaximumLength(128);
    }
}

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Title).MaximumLength(100);
        RuleFor(x => x.Bio).MaximumLength(1000);
        RuleFor(x => x.AvatarPath).SitePath();
        RuleFor(x => x.CoachExperience).MaximumLength(50);
        RuleFor(x => x.CoachStory).MaximumLength(2000);
        RuleFor(x => x.CoachVideoUrl!).HttpsUrl().When(x => !string.IsNullOrWhiteSpace(x.CoachVideoUrl));
        RuleFor(x => x.CoachVideoCoverPath).SitePath();
    }
}

// Uzunluk ve karakter kuralları Identity'de (Program.cs → Password ayarları)
public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().MaximumLength(128);
        RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(128)
            .NotEqual(x => x.CurrentPassword).WithMessage("Yeni şifre mevcut şifreyle aynı olamaz.");
    }
}
