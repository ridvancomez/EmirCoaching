using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using FluentValidation;

namespace EmirCoaching.Business.Services.Concrete;

public class FaqService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateFaqDto> createValidator,
    IValidator<UpdateFaqDto> updateValidator) : IFaqService
{
    public async Task<IEnumerable<FaqDto>> GetByGroupAsync(FaqGroup group) =>
        mapper.Map<List<FaqDto>>(await unitOfWork.Faqs.GetByGroupAsync(group));

    public async Task<IEnumerable<FaqDto>> GetAllAsync()
    {
        var faqs = await unitOfWork.Faqs.GetAllAsync();
        return mapper.Map<List<FaqDto>>(faqs.OrderBy(f => f.Group).ThenBy(f => f.SortOrder));
    }

    public async Task<FaqDto?> GetByIdAsync(int id)
    {
        var faq = await unitOfWork.Faqs.GetByIdAsync(id);
        return faq is null ? null : mapper.Map<FaqDto>(faq);
    }

    public async Task<Result<int>> CreateAsync(CreateFaqDto dto)
    {
        var errors = await createValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        var faq = mapper.Map<Faq>(dto);
        await unitOfWork.Faqs.AddAsync(faq);
        await unitOfWork.SaveChangesAsync();
        return Result<int>.Success(faq.Id);
    }

    public async Task<Result> UpdateAsync(UpdateFaqDto dto)
    {
        var errors = await updateValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var faq = await unitOfWork.Faqs.GetByIdAsync(dto.Id);
        if (faq is null)
            return Result.NotFound();

        mapper.Map(dto, faq);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var faq = await unitOfWork.Faqs.GetByIdAsync(id);
        if (faq is null)
            return Result.NotFound();

        unitOfWork.Faqs.Remove(faq);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
