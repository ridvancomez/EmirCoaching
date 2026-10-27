using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateTransformationTypeRequestValidator : AbstractValidator<CreateTransformationTypeRequest>
{
    public CreateTransformationTypeRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).Slug();
    }
}

public class UpdateTransformationTypeRequestValidator()
    : UpdateRequestValidator<UpdateTransformationTypeRequest, CreateTransformationTypeRequest>(new CreateTransformationTypeRequestValidator());
