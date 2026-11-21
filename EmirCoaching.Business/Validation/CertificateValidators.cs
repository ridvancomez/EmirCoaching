using EmirCoaching.Business.Dtos;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreateCertificateRequestValidator : AbstractValidator<CreateCertificateRequest>
{
    private const int MinYear = 1950;

    public CreateCertificateRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Issuer).NotEmpty().MaximumLength(50);
        RuleFor(x => x.IssuerFullName).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Detail).MaximumLength(100);
        RuleFor(x => x.Year).InclusiveBetween(MinYear, DateTime.UtcNow.Year);
        RuleFor(x => x.LogoPath).NotEmpty().SitePath();
    }
}

public class UpdateCertificateRequestValidator()
    : UpdateRequestValidator<UpdateCertificateRequest, CreateCertificateRequest>(new CreateCertificateRequestValidator());
