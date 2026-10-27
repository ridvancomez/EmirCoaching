using EmirCoaching.Business.Common;
using FluentValidation;

namespace EmirCoaching.Business.Validation;

// Update{Entity}Request = Create{Entity}Request + Id: oluşturma kuralları aynen uygulanır, Id kontrolü eklenir
public abstract class UpdateRequestValidator<TUpdateRequest, TCreateRequest> : AbstractValidator<TUpdateRequest>
    where TCreateRequest : class
    where TUpdateRequest : class, TCreateRequest, IUpdateRequest
{
    protected UpdateRequestValidator(IValidator<TCreateRequest> createValidator)
    {
        Include(createValidator);
        RuleFor(x => x.Id).GreaterThan(0);
    }
}
