using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateFaqDtoValidator : AbstractValidator<CreateFaqDto>
{
    public CreateFaqDtoValidator()
    {
        RuleFor(x => x.Question).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Answer).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Group).IsInEnum();
    }
}

public class UpdateFaqDtoValidator : AbstractValidator<UpdateFaqDto>
{
    public UpdateFaqDtoValidator()
    {
        Include(new CreateFaqDtoValidator());
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

public class CreateSiteStatDtoValidator : AbstractValidator<CreateSiteStatDto>
{
    public CreateSiteStatDtoValidator()
    {
        RuleFor(x => x.Label).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Value).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Suffix).MaximumLength(20);
    }
}

public class UpdateSiteStatDtoValidator : AbstractValidator<UpdateSiteStatDto>
{
    public UpdateSiteStatDtoValidator()
    {
        Include(new CreateSiteStatDtoValidator());
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

public class UpdateSiteSettingDtoValidator : AbstractValidator<UpdateSiteSettingDto>
{
    public UpdateSiteSettingDtoValidator()
    {
        RuleFor(x => x.WhatsApp).NotEmpty().Matches("^[0-9]{10,15}$")
            .WithMessage("WhatsApp numarası ülke koduyla, yalnızca rakam olmalı (örn. 905xxxxxxxxx).");
        RuleFor(x => x.Phone).Phone();
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
        RuleFor(x => x.WorkingHours).NotEmpty().MaximumLength(300);

        RuleFor(x => x.Instagram).Must(BeHttpsUrl!).When(x => x.Instagram is not null).WithMessage("Geçerli bir https adresi girin.");
        RuleFor(x => x.Facebook).Must(BeHttpsUrl!).When(x => x.Facebook is not null).WithMessage("Geçerli bir https adresi girin.");
        RuleFor(x => x.LinkedIn).Must(BeHttpsUrl!).When(x => x.LinkedIn is not null).WithMessage("Geçerli bir https adresi girin.");
        RuleFor(x => x.Twitter).Must(BeHttpsUrl!).When(x => x.Twitter is not null).WithMessage("Geçerli bir https adresi girin.");
    }

    private static bool BeHttpsUrl(string url) =>
        url.Length <= 300 && Uri.TryCreate(url, UriKind.Absolute, out var uri) && uri.Scheme == Uri.UriSchemeHttps;
}
