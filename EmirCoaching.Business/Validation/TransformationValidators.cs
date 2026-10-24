using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateTransformationDtoValidator : AbstractValidator<CreateTransformationDto>
{
    public CreateTransformationDtoValidator()
    {
        RuleFor(x => x.ClientName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Age).InclusiveBetween(10, 100);
        RuleFor(x => x.DurationWeeks).InclusiveBetween(1, 520);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Quote).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Story).NotEmpty();
        RuleFor(x => x.BeforeImagePath).NotEmpty().SitePath();
        RuleFor(x => x.AfterImagePath).NotEmpty().SitePath();
        RuleFor(x => x.BeforeSquarePath).SitePath();
        RuleFor(x => x.AfterSquarePath).SitePath();
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100);
        RuleForEach(x => x.Metrics).ChildRules(metric =>
        {
            metric.RuleFor(m => m.Label).NotEmpty().MaximumLength(50);
            metric.RuleFor(m => m.Value).NotEmpty().MaximumLength(30);
        });
    }
}

public class UpdateTransformationDtoValidator : AbstractValidator<UpdateTransformationDto>
{
    public UpdateTransformationDtoValidator()
    {
        Include(new CreateTransformationDtoValidator());
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
