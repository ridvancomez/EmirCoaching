using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;

namespace EmirCoaching.Business.Services.Concrete;

public class SiteSettingService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<UpdateSiteSettingDto> validator) : ISiteSettingService
{
    private const int SettingId = 1; // SeedData ile oluşturulan tek kayıt

    public async Task<SiteSettingDto?> GetAsync()
    {
        var setting = await unitOfWork.Repository<SiteSetting>().GetByIdAsync(SettingId);
        return setting is null ? null : mapper.Map<SiteSettingDto>(setting);
    }

    public async Task<Result> UpdateAsync(UpdateSiteSettingDto dto)
    {
        var errors = await validator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var setting = await unitOfWork.Repository<SiteSetting>().GetByIdAsync(SettingId);
        if (setting is null)
            return Result.NotFound("Site ayarları bulunamadı; seed verisi eksik.");

        mapper.Map(dto, setting);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
