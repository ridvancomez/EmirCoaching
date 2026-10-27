using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class UpdateDietProgramRequestValidator : AbstractValidator<UpdateDietProgramRequest>
{
    public UpdateDietProgramRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Price).GreaterThan(0).PrecisionScale(10, 2, ignoreTrailingZeros: true);
        RuleFor(x => x.DurationWeeks).InclusiveBetween(1, 52);
        RuleFor(x => x.DietitianTitle).NotEmpty().MaximumLength(50);
        RuleFor(x => x.DietitianFullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DietitianPhotoPath).SitePath();
    }
}

public class CreateDietProgramStepRequestValidator : AbstractValidator<CreateDietProgramStepRequest>
{
    public CreateDietProgramStepRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Icon).IsInEnum();
    }
}

public class UpdateDietProgramStepRequestValidator()
    : UpdateRequestValidator<UpdateDietProgramStepRequest, CreateDietProgramStepRequest>(new CreateDietProgramStepRequestValidator());
