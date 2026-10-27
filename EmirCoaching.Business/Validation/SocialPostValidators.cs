using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateSocialPostRequestValidator : AbstractValidator<CreateSocialPostRequest>
{
    public CreateSocialPostRequestValidator()
    {
        RuleFor(x => x.Platform).IsInEnum();
        RuleFor(x => x.Title).MaximumLength(150);
        RuleFor(x => x.Caption).MaximumLength(2200);
        RuleFor(x => x.Hashtag).MaximumLength(100);
        RuleFor(x => x.ImagePath).NotEmpty().SitePath();
        RuleFor(x => x.Url).NotEmpty().HttpsUrl();
    }
}

public class UpdateSocialPostRequestValidator()
    : UpdateRequestValidator<UpdateSocialPostRequest, CreateSocialPostRequest>(new CreateSocialPostRequestValidator());
