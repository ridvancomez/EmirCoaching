using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateContactMessageRequestValidator : AbstractValidator<CreateContactMessageRequest>
{
    private static readonly string[] Locations = ["evde", "salonda"];

    public CreateContactMessageRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Phone).Phone();
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Location).Must(l => l is null || Locations.Contains(l))
            .WithMessage("Konum 'evde' veya 'salonda' olmalı.");
        RuleFor(x => x.Message).MaximumLength(2000);
        RuleFor(x => x.PrivacyAccepted).Equal(true).WithMessage("Gizlilik Politikası onayı zorunludur.");
    }
}
