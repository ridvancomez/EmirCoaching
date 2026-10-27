using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateFaqRequestValidator : AbstractValidator<CreateFaqRequest>
{
    public CreateFaqRequestValidator()
    {
        RuleFor(x => x.Question).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Answer).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Group).IsInEnum();
    }
}

public class UpdateFaqRequestValidator()
    : UpdateRequestValidator<UpdateFaqRequest, CreateFaqRequest>(new CreateFaqRequestValidator());
