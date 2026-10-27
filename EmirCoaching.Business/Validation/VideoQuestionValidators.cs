using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateVideoQuestionRequestValidator : AbstractValidator<CreateVideoQuestionRequest>
{
    public CreateVideoQuestionRequestValidator()
    {
        RuleFor(x => x.Question).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.ThumbnailPath).NotEmpty().SitePath();
        RuleFor(x => x.YouTubeUrl).YouTubeUrl();
    }
}

public class UpdateVideoQuestionRequestValidator()
    : UpdateRequestValidator<UpdateVideoQuestionRequest, CreateVideoQuestionRequest>(new CreateVideoQuestionRequestValidator());
