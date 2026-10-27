using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateBlogCategoryRequestValidator : AbstractValidator<CreateBlogCategoryRequest>
{
    public CreateBlogCategoryRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).Slug();
    }
}

public class UpdateBlogCategoryRequestValidator()
    : UpdateRequestValidator<UpdateBlogCategoryRequest, CreateBlogCategoryRequest>(new CreateBlogCategoryRequestValidator());
