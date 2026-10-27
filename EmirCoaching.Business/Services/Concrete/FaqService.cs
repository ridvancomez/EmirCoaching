using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class FaqService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateFaqRequest> createValidator,
    IValidator<UpdateFaqRequest> updateValidator,
    ILogger<FaqService> logger)
    : CrudService<Faq, FaqResponse, CreateFaqRequest, UpdateFaqRequest>(
        unitOfWork.Faqs, unitOfWork, mapper, createValidator, updateValidator, logger), IFaqService
{
    public async Task<Result<IReadOnlyList<FaqResponse>>> GetByGroupAsync(FaqGroup group) =>
        Result<IReadOnlyList<FaqResponse>>.Success(Mapper.Map<List<FaqResponse>>(await UnitOfWork.Faqs.GetByGroupAsync(group)));

    // Grup, sonra SortOrder (OrderBy kararlı olduğu için grup içi sıra korunur)
    protected override async Task<IReadOnlyList<Faq>> ListAsync() =>
        (await UnitOfWork.Faqs.GetOrderedAsync()).OrderBy(f => f.Group).ToList();
}
