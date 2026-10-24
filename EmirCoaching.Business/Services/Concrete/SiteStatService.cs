using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;

namespace EmirCoaching.Business.Services.Concrete;

public class SiteStatService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateSiteStatDto> createValidator,
    IValidator<UpdateSiteStatDto> updateValidator) : ISiteStatService
{
    private IRepository<SiteStat> Stats => unitOfWork.Repository<SiteStat>();

    public async Task<IEnumerable<SiteStatDto>> GetAllAsync()
    {
        var stats = await Stats.GetAllAsync();
        return mapper.Map<List<SiteStatDto>>(stats.OrderBy(s => s.SortOrder));
    }

    public async Task<SiteStatDto?> GetByIdAsync(int id)
    {
        var stat = await Stats.GetByIdAsync(id);
        return stat is null ? null : mapper.Map<SiteStatDto>(stat);
    }

    public async Task<Result<int>> CreateAsync(CreateSiteStatDto dto)
    {
        var errors = await createValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        var stat = mapper.Map<SiteStat>(dto);
        await Stats.AddAsync(stat);
        await unitOfWork.SaveChangesAsync();
        return Result<int>.Success(stat.Id);
    }

    public async Task<Result> UpdateAsync(UpdateSiteStatDto dto)
    {
        var errors = await updateValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var stat = await Stats.GetByIdAsync(dto.Id);
        if (stat is null)
            return Result.NotFound();

        mapper.Map(dto, stat);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var stat = await Stats.GetByIdAsync(id);
        if (stat is null)
            return Result.NotFound();

        Stats.Remove(stat);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
