using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class NewsletterEmailValidator : AbstractValidator<string>
{
    public NewsletterEmailValidator()
    {
        RuleFor(x => x).NotEmpty().EmailAddress().MaximumLength(256)
            .WithName("E-posta").WithMessage("Geçerli bir e-posta adresi girin.");
    }
}
