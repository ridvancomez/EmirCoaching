using AutoMapper;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class DietProgramStepService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateDietProgramStepRequest> createValidator,
    IValidator<UpdateDietProgramStepRequest> updateValidator,
    ILogger<DietProgramStepService> logger)
    : CrudService<DietProgramStep, DietProgramStepResponse, CreateDietProgramStepRequest, UpdateDietProgramStepRequest>(
        unitOfWork.DietProgramSteps, unitOfWork, mapper, createValidator, updateValidator, logger), IDietProgramStepService
{
    protected override Task<IReadOnlyList<DietProgramStep>> ListAsync() => UnitOfWork.DietProgramSteps.GetOrderedAsync();
}
