using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Services.Abstract;

// Denemeler AuthService'te kaydedilir; burada yalnızca admin listesi
public interface ILoginAttemptService
{
    Task<Result<PagedResult<LoginAttemptResponse>>> GetPagedAsync(LoginAttemptResult? result, int page, int size);
}
