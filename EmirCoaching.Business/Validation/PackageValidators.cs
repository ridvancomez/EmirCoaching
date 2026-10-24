using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

public class CreatePackageDtoValidator : AbstractValidator<CreatePackageDto>
{
    public CreatePackageDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Slug).Slug();
        RuleFor(x => x.Subtitle).NotEmpty().MaximumLength(150);
        RuleFor(x => x.DurationMonths).InclusiveBetween(1, 36);
        RuleFor(x => x.Price).GreaterThan(0).PrecisionScale(10, 2, ignoreTrailingZeros: true);
        RuleFor(x => x.Location).IsInEnum();
        RuleFor(x => x.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.MetaTitle).MaximumLength(70);
        RuleFor(x => x.MetaDescription).MaximumLength(170);

        RuleForEach(x => x.Highlights).NotEmpty().MaximumLength(200);
        RuleForEach(x => x.Modules).SetValidator(new PackageModuleInputDtoValidator());
    }
}

public class UpdatePackageDtoValidator : AbstractValidator<UpdatePackageDto>
{
    public UpdatePackageDtoValidator()
    {
        Include(new CreatePackageDtoValidator());
        RuleFor(x => x.Id).GreaterThan(0);
    }
}

public class PackageModuleInputDtoValidator : AbstractValidator<PackageModuleInputDto>
{
    public PackageModuleInputDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.IconKey).NotEmpty().MaximumLength(50);
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Text).NotEmpty().MaximumLength(200);
            item.RuleFor(i => i.State).IsInEnum();
            item.RuleFor(i => i.LockedLabel).MaximumLength(100);
            item.RuleFor(i => i.LockedLabel).NotEmpty()
                .When(i => i.State == ModuleItemState.Kilitli)
                .WithMessage("Kilitli kalem için etiket (örn. \"6 ve 12 Aylık Pakette\") zorunludur.");
        });
    }
}
