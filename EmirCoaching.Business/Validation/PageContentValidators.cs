using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class UpdatePageContentRequestValidator : AbstractValidator<UpdatePageContentRequest>
{
    public UpdatePageContentRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.HighlightText).MaximumLength(50)
            .Must((request, highlight) => highlight is null || request.Title.Contains(highlight))
            .WithMessage("Vurgulanacak metin başlığın içinde geçmeli.");
        RuleFor(x => x.Description).MaximumLength(500);
        RuleFor(x => x.BackgroundImagePath).SitePath();
        RuleFor(x => x.ButtonUrl).MaximumLength(300);
    }
}
