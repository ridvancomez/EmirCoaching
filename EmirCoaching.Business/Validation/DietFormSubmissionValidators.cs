using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

// Aralıklar front-end diyet-formu.html input sınırlarıyla aynı
public class CreateDietFormSubmissionRequestValidator : AbstractValidator<CreateDietFormSubmissionRequest>
{
    public CreateDietFormSubmissionRequestValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Gender).IsInEnum();
        RuleFor(x => x.Age).InclusiveBetween(10, 100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.PhoneNumber).Phone();
        RuleFor(x => x.HeightCm).InclusiveBetween(100, 250);
        RuleFor(x => x.CurrentWeight).InclusiveBetween(30, 300);
        RuleFor(x => x.TargetWeight).InclusiveBetween(30, 300);
        RuleFor(x => x.MealsPerDay).InclusiveBetween(2, 6);
        RuleFor(x => x.Allergies).MaximumLength(500);
        RuleFor(x => x.AvoidedFoods).MaximumLength(500);
        RuleFor(x => x.Supplements).MaximumLength(500);
        RuleFor(x => x.SleepHours).InclusiveBetween(0, 24);
        RuleFor(x => x.ActivityLevel).IsInEnum();
        RuleFor(x => x.WaterLiters).InclusiveBetween(0, 10);
        RuleFor(x => x.PrivacyAccepted).Equal(true).WithMessage("Gizlilik Politikası onayı zorunludur.");
    }
}
