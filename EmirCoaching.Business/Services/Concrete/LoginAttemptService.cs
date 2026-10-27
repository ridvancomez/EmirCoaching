using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Mapping;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Identity;

namespace EmirCoaching.Business.Services.Concrete;

public class LoginAttemptService(IUnitOfWork unitOfWork, IMapper mapper) : ILoginAttemptService
{
    public async Task<Result<PagedResult<LoginAttemptResponse>>> GetPagedAsync(LoginAttemptResult? result, int page, int size)
    {
        var attempts = await unitOfWork.LoginAttempts.GetPagedAsync(result, page, size);
        return Result<PagedResult<LoginAttemptResponse>>.Success(mapper.MapPaged<LoginAttempt, LoginAttemptResponse>(attempts));
    }
}
