using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class UpdateSiteSettingRequestValidator : AbstractValidator<UpdateSiteSettingRequest>
{
    public UpdateSiteSettingRequestValidator()
    {
        RuleFor(x => x.Phone).Phone();
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.LegalEmail).NotEmpty().EmailAddress().MaximumLength(256);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
        RuleFor(x => x.WorkingDays1).NotEmpty().MaximumLength(50);
        RuleFor(x => x.WorkingHours1).NotEmpty().MaximumLength(50);
        RuleFor(x => x.WorkingDays2).NotEmpty().MaximumLength(50);
        RuleFor(x => x.WorkingHours2).NotEmpty().MaximumLength(50);
    }
}
