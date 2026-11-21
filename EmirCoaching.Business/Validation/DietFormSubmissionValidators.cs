using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

// Aralıklar front-end diyet-formu.html input sınırlarıyla aynı
public class CreateDietFormSubmissionRequestValidator : AbstractValidator<CreateDietFormSubmissionRequest>
{
    public CreateDietFormSubmissionRequestValidator()
    {
        RuleFor(x => x.PackageId).GreaterThan(0);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Gender).IsInEnum();
        RuleFor(x => x.Age).InclusiveBetween(10, 100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.PhoneNumber).Phone();
        RuleFor(x => x.HeightCm).InclusiveBetween(100, 250);
        RuleFor(x => x.CurrentWeight).InclusiveBetween(30, 300);
        RuleFor(x => x.TargetWeight).InclusiveBetween(30, 300);
        RuleFor(x => x.PrivacyAccepted).Equal(true).WithMessage("Gizlilik Politikası onayı zorunludur.");
    }
}

public class UpdateDietFormSubmissionClientFormRequestValidator : AbstractValidator<UpdateDietFormSubmissionClientFormRequest>
{
    public UpdateDietFormSubmissionClientFormRequestValidator()
    {
        RuleFor(x => x.ClientFormUrl!).GoogleFormUrl().When(x => !string.IsNullOrWhiteSpace(x.ClientFormUrl));
    }
}
