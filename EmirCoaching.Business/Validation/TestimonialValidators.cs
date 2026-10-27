using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateTestimonialRequestValidator : AbstractValidator<CreateTestimonialRequest>
{
    public CreateTestimonialRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Text).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Rating).Rating();
        RuleFor(x => x.PackageId).NotEmpty();
    }
}

public class UpdateTestimonialRequestValidator()
    : UpdateRequestValidator<UpdateTestimonialRequest, CreateTestimonialRequest>(new CreateTestimonialRequestValidator());
