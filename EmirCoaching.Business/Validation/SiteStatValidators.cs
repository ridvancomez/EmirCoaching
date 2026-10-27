using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateSiteStatRequestValidator : AbstractValidator<CreateSiteStatRequest>
{
    public CreateSiteStatRequestValidator()
    {
        RuleFor(x => x.Label).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Value).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Suffix).MaximumLength(20);
    }
}

public class UpdateSiteStatRequestValidator()
    : UpdateRequestValidator<UpdateSiteStatRequest, CreateSiteStatRequest>(new CreateSiteStatRequestValidator());
