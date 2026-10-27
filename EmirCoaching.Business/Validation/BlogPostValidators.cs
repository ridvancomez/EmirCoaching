using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateBlogPostRequestValidator : AbstractValidator<CreateBlogPostRequest>
{
    public CreateBlogPostRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Slug).Slug();
        RuleFor(x => x.Summary).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ContentHtml).NotEmpty();
        RuleFor(x => x.CoverImagePath).SitePath();
        RuleFor(x => x.CoverImageCaption).MaximumLength(200);
        RuleFor(x => x.Tags).Must(t => t.Count <= 10).WithMessage("En fazla 10 etiket eklenebilir.");
        RuleForEach(x => x.Tags).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ReadingMinutes).InclusiveBetween(1, 120);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.MetaTitle).MaximumLength(70);
        RuleFor(x => x.MetaDescription).MaximumLength(170);
    }
}

public class UpdateBlogPostRequestValidator()
    : UpdateRequestValidator<UpdateBlogPostRequest, CreateBlogPostRequest>(new CreateBlogPostRequestValidator());
