using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class CoachProfileService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CoachProfileService> logger) : ICoachProfileService
{
    public async Task<Result<CoachProfileResponse>> GetAsync()
    {
        var coach = await unitOfWork.Users.GetCoachAsync();
        if (coach is null)
        {
            logger.LogWarning("Koç olarak işaretli kullanıcı yok");
            return Result<CoachProfileResponse>.NotFound("Koç profili bulunamadı.");
        }

        return Result<CoachProfileResponse>.Success(mapper.Map<CoachProfileResponse>(coach));
    }
}
