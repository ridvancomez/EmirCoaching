using AutoMapper;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class PurchaseStepService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreatePurchaseStepRequest> createValidator,
    IValidator<UpdatePurchaseStepRequest> updateValidator,
    ILogger<PurchaseStepService> logger)
    : CrudService<PurchaseStep, PurchaseStepResponse, CreatePurchaseStepRequest, UpdatePurchaseStepRequest>(
        unitOfWork.PurchaseSteps, unitOfWork, mapper, createValidator, updateValidator, logger), IPurchaseStepService
{
    protected override Task<IReadOnlyList<PurchaseStep>> ListAsync() => UnitOfWork.PurchaseSteps.GetOrderedAsync();
}
