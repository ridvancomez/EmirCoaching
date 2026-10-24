using FluentValidation;

namespace EmirCoaching.Business.Common;

public static class ValidationExtensions
{
    public static async Task<List<string>> GetErrorsAsync<T>(this IValidator<T> validator, T instance)
    {
        var result = await validator.ValidateAsync(instance);
        return result.Errors.Select(e => e.ErrorMessage).ToList();
    }
}
