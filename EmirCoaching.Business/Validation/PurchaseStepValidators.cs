using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreatePurchaseStepRequestValidator : AbstractValidator<CreatePurchaseStepRequest>
{
    public CreatePurchaseStepRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.TimeLabel).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IconKey).NotEmpty().MaximumLength(50);
    }
}

public class UpdatePurchaseStepRequestValidator()
    : UpdateRequestValidator<UpdatePurchaseStepRequest, CreatePurchaseStepRequest>(new CreatePurchaseStepRequestValidator());
