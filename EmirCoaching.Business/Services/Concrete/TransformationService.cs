using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class TransformationService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateTransformationRequest> createValidator,
    IValidator<UpdateTransformationRequest> updateValidator,
    IUploadCleanupService uploadCleanup,
    ILogger<TransformationService> logger) : ITransformationService
{
    private const string TransformationNotFound = "Değişim bulunamadı.";

    public async Task<Result<IReadOnlyList<TransformationResponse>>> GetListAsync(string? typeSlug, string? sort)
    {
        var order = sort == "en-eskiler" ? TransformationSort.EnEskiler : TransformationSort.EnYeniler;
        var transformations = await unitOfWork.Transformations.GetListAsync(typeSlug, order);
        return Result<IReadOnlyList<TransformationResponse>>.Success(mapper.Map<List<TransformationResponse>>(transformations));
    }

    public async Task<Result<IReadOnlyList<TransformationResponse>>> GetHomeAsync() =>
        Result<IReadOnlyList<TransformationResponse>>.Success(
            mapper.Map<List<TransformationResponse>>(await unitOfWork.Transformations.GetForHomeAsync()));

    // Site: pasif değişim bulunamadı sayılır
    public Task<Result<TransformationDetailResponse>> GetDetailAsync(int id) => GetDetailAsync(id, activeOnly: true);

    public async Task<Result<IReadOnlyList<TransformationResponse>>> GetAllAsync() =>
        Result<IReadOnlyList<TransformationResponse>>.Success(
            mapper.Map<List<TransformationResponse>>(await unitOfWork.Transformations.GetOrderedListAsync()));

    public Task<Result<TransformationDetailResponse>> GetByIdAsync(int id) => GetDetailAsync(id, activeOnly: false);

    private async Task<Result<TransformationDetailResponse>> GetDetailAsync(int id, bool activeOnly)
    {
        var transformation = await unitOfWork.Transformations.GetWithMetricsAsync(id);
        if (transformation is null || (activeOnly && !transformation.IsActive))
        {
            logger.LogWarning("Değişim {Id} bulunamadı", id);
            return Result<TransformationDetailResponse>.NotFound(TransformationNotFound);
        }

        return Result<TransformationDetailResponse>.Success(mapper.Map<TransformationDetailResponse>(transformation) with
        {
            Metrics = mapper.Map<List<TransformationMetricResponse>>(transformation.Metrics.OrderBy(m => m.SortOrder))
        });
    }

    public async Task<Result<int>> CreateAsync(CreateTransformationRequest request)
    {
        var errors = await createValidator.GetErrorsAsync(request);
        errors.AddRange(await CheckReferencesAsync(request));
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        var transformation = mapper.Map<Transformation>(request);
        SyncMetrics(transformation, request);

        await unitOfWork.Transformations.AddAsync(transformation);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Değişim {Id} oluşturuldu", transformation.Id);
        return Result<int>.Success(transformation.Id);
    }

    public async Task<Result> UpdateAsync(UpdateTransformationRequest request)
    {
        var errors = await updateValidator.GetErrorsAsync(request);
        errors.AddRange(await CheckReferencesAsync(request));
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var transformation = await unitOfWork.Transformations.GetWithMetricsAsync(request.Id);
        if (transformation is null)
            return Result.NotFound(TransformationNotFound);

        if (!ChildListSync.AreKnownIds(request.Metrics.Select(m => m.Id), transformation.Metrics.Select(m => m.Id)))
            return Result.Invalid("Gönderilen ölçümlerden biri bu değişime ait değil veya tekrar ediyor.");

        var oldUploads = UploadPaths.Of(transformation);
        mapper.Map(request, transformation);
        SyncMetrics(transformation, request);

        await unitOfWork.SaveChangesAsync();
        await uploadCleanup.DeleteUnusedAsync(oldUploads.Except(UploadPaths.Of(transformation)));

        logger.LogInformation("Değişim {Id} güncellendi", transformation.Id);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var transformation = await unitOfWork.Transformations.GetWithMetricsAsync(id);
        if (transformation is null)
            return Result.NotFound(TransformationNotFound);

        unitOfWork.Transformations.Remove(transformation);
        await unitOfWork.SaveChangesAsync();
        await uploadCleanup.DeleteUnusedAsync(UploadPaths.Of(transformation));

        logger.LogInformation("Değişim {Id} silindi", id);
        return Result.Success();
    }

    public async Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids)
    {
        var loaded = await BulkDelete.LoadAsync(ids, unitOfWork.Transformations.GetWithMetricsAsync, "Seçilen değişimlerden biri bulunamadı.");
        if (!loaded.IsSuccess)
            return loaded;

        foreach (var transformation in loaded.Value!)
            unitOfWork.Transformations.Remove(transformation);
        await unitOfWork.SaveChangesAsync();
        await uploadCleanup.DeleteUnusedAsync(loaded.Value!.SelectMany(UploadPaths.Of));

        logger.LogInformation("{Count} değişim silindi: {Ids}", loaded.Value!.Count, loaded.Value!.Select(t => t.Id));
        return Result.Success();
    }

    private async Task<List<string>> CheckReferencesAsync(CreateTransformationRequest request)
    {
        var errors = new List<string>();

        if (await unitOfWork.TransformationTypes.GetByIdAsync(request.TransformationTypeId) is null)
            errors.Add("Dönüşüm tipi bulunamadı.");

        return errors;
    }

    // Ölçümler gönderilen listeyle eşitlenir; numaraları korunur, listeden çıkan soft delete olur
    private void SyncMetrics(Transformation transformation, CreateTransformationRequest request) =>
        ChildListSync.Sync(transformation.Metrics, request.Metrics, m => m.Id, unitOfWork.TransformationMetrics.Remove,
            (metric, input, order) =>
            {
                metric.Label = input.Label;
                metric.Value = input.Value;
                metric.SortOrder = order;
            });
}
