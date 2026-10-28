using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class TransformationTypeService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateTransformationTypeRequest> createValidator,
    IValidator<UpdateTransformationTypeRequest> updateValidator,
    ILogger<TransformationTypeService> logger)
    : CrudService<TransformationType, TransformationTypeResponse, CreateTransformationTypeRequest, UpdateTransformationTypeRequest>(
        unitOfWork.TransformationTypes, unitOfWork, mapper, createValidator, updateValidator, logger), ITransformationTypeService
{
    protected override Task<IReadOnlyList<TransformationType>> ListAsync() => UnitOfWork.TransformationTypes.GetOrderedAsync();

    protected override async Task<Result> CheckAsync(CreateTransformationTypeRequest request, int? id) =>
        await UnitOfWork.TransformationTypes.SlugExistsAsync(request.Slug, id)
            ? Result.Conflict("Bu slug başka bir dönüşüm tipinde kullanılıyor.")
            : Result.Success();

    protected override async Task<Result> CheckDeleteAsync(TransformationType type) =>
        await UnitOfWork.Transformations.AnyWithTypeAsync(type.Id)
            ? Result.Conflict($"'{type.Name}' tipinde değişimler var; önce değişimlerin tipini değiştirin.")
            : Result.Success();
}
