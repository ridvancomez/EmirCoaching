using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateBlogPostDtoValidator : AbstractValidator<CreateBlogPostDto>
{
    public CreateBlogPostDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Slug).Slug();
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ContentHtml).NotEmpty();
        RuleFor(x => x.CoverImagePath).SitePath();
        RuleFor(x => x.ReadingMinutes).InclusiveBetween(1, 120);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.MetaTitle).MaximumLength(70);
        RuleFor(x => x.MetaDescription).MaximumLength(170);
    }
}

public class UpdateBlogPostDtoValidator : AbstractValidator<UpdateBlogPostDto>
{
    public UpdateBlogPostDtoValidator()
    {
        Include(new CreateBlogPostDtoValidator());
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
