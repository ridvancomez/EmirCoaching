using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateSocialLinkRequestValidator : AbstractValidator<CreateSocialLinkRequest>
{
    public CreateSocialLinkRequestValidator()
    {
        RuleFor(x => x.Platform).IsInEnum();
        RuleFor(x => x.Url).NotEmpty().HttpsUrl();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(200);
    }
}

public class UpdateSocialLinkRequestValidator()
    : UpdateRequestValidator<UpdateSocialLinkRequest, CreateSocialLinkRequest>(new CreateSocialLinkRequestValidator());
