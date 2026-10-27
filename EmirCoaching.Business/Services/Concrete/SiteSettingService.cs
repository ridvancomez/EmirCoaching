using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class SiteSettingService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<UpdateSiteSettingRequest> validator,
    ILogger<SiteSettingService> logger) : ISiteSettingService
{
    private const int SettingId = 1; // SeedData ile oluşturulan tek kayıt
    private const string SettingMissing = "Site ayarları bulunamadı; seed verisi eksik.";

    public async Task<Result<SiteSettingResponse>> GetAsync()
    {
        var setting = await unitOfWork.SiteSettings.GetByIdAsync(SettingId);
        return setting is null
            ? Result<SiteSettingResponse>.NotFound(SettingMissing)
            : Result<SiteSettingResponse>.Success(mapper.Map<SiteSettingResponse>(setting));
    }

    public async Task<Result> UpdateAsync(UpdateSiteSettingRequest request)
    {
        var errors = await validator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var setting = await unitOfWork.SiteSettings.GetByIdAsync(SettingId);
        if (setting is null)
            return Result.NotFound(SettingMissing);

        mapper.Map(request, setting);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Site ayarları güncellendi");
        return Result.Success();
    }
}
