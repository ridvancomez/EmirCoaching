using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class PublishLegalDocumentRequestValidator : AbstractValidator<PublishLegalDocumentRequest>
{
    public PublishLegalDocumentRequestValidator()
    {
        RuleFor(x => x.ContentHtml).NotEmpty();
        RuleFor(x => x.Note).NotEmpty().MaximumLength(300);
    }
}
