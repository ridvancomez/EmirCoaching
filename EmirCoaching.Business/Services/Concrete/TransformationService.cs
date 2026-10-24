using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using FluentValidation;

namespace EmirCoaching.Business.Services.Concrete;

public class TransformationService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateTransformationDto> createValidator,
    IValidator<UpdateTransformationDto> updateValidator) : ITransformationService
{
    public async Task<IEnumerable<TransformationDto>> GetListAsync(TransformationCategory? category, string? sort)
    {
        var order = sort == "en-eskiler" ? TransformationSort.EnEskiler : TransformationSort.EnYeniler;
        return mapper.Map<List<TransformationDto>>(await unitOfWork.Transformations.GetListAsync(category, order));
    }

    public async Task<IEnumerable<TransformationDto>> GetHomeAsync() =>
        mapper.Map<List<TransformationDto>>(await unitOfWork.Transformations.GetForHomeAsync());

    public async Task<TransformationDetailDto?> GetDetailAsync(int id)
    {
        var transformation = await unitOfWork.Transformations.GetWithMetricsAsync(id);
        if (transformation is null)
            return null;

        return mapper.Map<TransformationDetailDto>(transformation) with
        {
            Metrics = mapper.Map<List<TransformationMetricDto>>(transformation.Metrics.OrderBy(m => m.SortOrder))
        };
    }

    public async Task<Result<int>> CreateAsync(CreateTransformationDto dto)
    {
        var errors = await createValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        var transformation = mapper.Map<Transformation>(dto);
        AddMetrics(transformation, dto);

        await unitOfWork.Transformations.AddAsync(transformation);
        await unitOfWork.SaveChangesAsync();
        return Result<int>.Success(transformation.Id);
    }

    public async Task<Result> UpdateAsync(UpdateTransformationDto dto)
    {
        var errors = await updateValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var transformation = await unitOfWork.Transformations.GetWithMetricsAsync(dto.Id);
        if (transformation is null)
            return Result.NotFound();

        mapper.Map(dto, transformation);

        var metricRepository = unitOfWork.Repository<TransformationMetric>();
        foreach (var metric in transformation.Metrics.ToList())
            metricRepository.Remove(metric);

        AddMetrics(transformation, dto);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var transformation = await unitOfWork.Transformations.GetWithMetricsAsync(id);
        if (transformation is null)
            return Result.NotFound();

        unitOfWork.Transformations.Remove(transformation);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private static void AddMetrics(Transformation transformation, CreateTransformationDto dto)
    {
        for (var i = 0; i < dto.Metrics.Count; i++)
        {
            transformation.Metrics.Add(new TransformationMetric
            {
                Label = dto.Metrics[i].Label,
                Value = dto.Metrics[i].Value,
                SortOrder = i + 1
            });
        }
    }
}
