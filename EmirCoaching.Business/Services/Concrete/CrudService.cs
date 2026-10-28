using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

// Ortak CRUD akışı: doğrula → iş kuralı kontrolü (CheckAsync) → kaydet → logla.
// Türeyen servis yalnızca farklı olanı ezer: liste sırası, çakışma/ilişki kontrolleri, silme engeli.
public abstract class CrudService<TEntity, TResponse, TCreateRequest, TUpdateRequest>(
    IRepository<TEntity> repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<TCreateRequest> createValidator,
    IValidator<TUpdateRequest> updateValidator,
    ILogger logger) : ICrudService<TResponse, TCreateRequest, TUpdateRequest>
    where TEntity : BaseEntity
    where TCreateRequest : class
    where TUpdateRequest : TCreateRequest, IUpdateRequest
{
    private static readonly string EntityName = typeof(TEntity).Name;

    protected IUnitOfWork UnitOfWork => unitOfWork;
    protected IMapper Mapper => mapper;
    protected ILogger Logger => logger;

    public async Task<Result<IReadOnlyList<TResponse>>> GetAllAsync() =>
        Result<IReadOnlyList<TResponse>>.Success(mapper.Map<List<TResponse>>(await ListAsync()));

    public async Task<Result<TResponse>> GetByIdAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound<TResponse>(id);

        return Result<TResponse>.Success(mapper.Map<TResponse>(entity));
    }

    public async Task<Result<int>> CreateAsync(TCreateRequest request)
    {
        var check = await ValidateAsync(createValidator, request, id: null);
        if (!check.IsSuccess)
            return Result<int>.Failure(check);

        var entity = mapper.Map<TEntity>(request);
        await repository.AddAsync(entity);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("{Entity} {Id} oluşturuldu", EntityName, entity.Id);
        return Result<int>.Success(entity.Id);
    }

    public async Task<Result> UpdateAsync(TUpdateRequest request)
    {
        var check = await ValidateAsync(updateValidator, request, request.Id);
        if (!check.IsSuccess)
            return check;

        var entity = await repository.GetByIdAsync(request.Id);
        if (entity is null)
            return NotFound<int>(request.Id);

        var oldUploads = UploadPaths.Of(entity);
        mapper.Map(request, entity);
        await unitOfWork.SaveChangesAsync();
        await ReleaseUploadsAsync(oldUploads.Except(UploadPaths.Of(entity)));

        logger.LogInformation("{Entity} {Id} güncellendi", EntityName, entity.Id);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null)
            return NotFound<int>(id);

        var check = await CheckDeleteAsync(entity);
        if (!check.IsSuccess)
        {
            logger.LogWarning("{Entity} {Id} silinemedi: {Errors}", EntityName, id, check.Errors);
            return check;
        }

        repository.Remove(entity);
        await unitOfWork.SaveChangesAsync();
        await ReleaseUploadsAsync(UploadPaths.Of(entity));

        logger.LogInformation("{Entity} {Id} silindi", EntityName, id);
        return Result.Success();
    }

    public async Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids)
    {
        var loaded = await BulkDelete.LoadAsync(ids, repository.GetByIdAsync, "Seçilen kayıtlardan biri bulunamadı.");
        if (!loaded.IsSuccess)
            return loaded;

        var blocked = new List<string>();
        foreach (var entity in loaded.Value!)
            blocked.AddRange((await CheckDeleteAsync(entity)).Errors);
        if (blocked.Count > 0)
        {
            logger.LogWarning("{Entity} toplu silme reddedildi: {Errors}", EntityName, blocked);
            return Result.Conflict(blocked);
        }

        foreach (var entity in loaded.Value!)
            repository.Remove(entity);
        await unitOfWork.SaveChangesAsync(); // tek SaveChanges = tek transaction
        await ReleaseUploadsAsync(loaded.Value!.SelectMany(UploadPaths.Of));

        logger.LogInformation("{Count} {Entity} kaydı silindi: {Ids}", loaded.Value!.Count, EntityName, loaded.Value!.Select(e => e.Id));
        return Result.Success();
    }

    // Admin listesinin kaynağı ve sırası; sıralı entity'ler GetOrderedAsync ile ezer
    protected virtual Task<IReadOnlyList<TEntity>> ListAsync() => repository.GetAllAsync();

    // Doğrulamadan sonra çalışan iş kuralları (slug/platform çakışması, ilişkili kayıt var mı); id güncellemede dolu
    protected virtual Task<Result> CheckAsync(TCreateRequest request, int? id) => Task.FromResult(Result.Success());

    // Silmeyi engelleyen durumlar (kayda bağlı başka kayıtlar varsa Conflict)
    protected virtual Task<Result> CheckDeleteAsync(TEntity entity) => Task.FromResult(Result.Success());

    // Güncellemede değişen veya silinen kayda ait yüklemeler (kaydettikten sonra); görsel tutan servis
    // IUploadCleanupService.DeleteUnusedAsync'e yönlendirir
    protected virtual Task ReleaseUploadsAsync(IEnumerable<string> paths) => Task.CompletedTask;

    private async Task<Result> ValidateAsync<TRequest>(IValidator<TRequest> validator, TRequest request, int? id)
        where TRequest : TCreateRequest
    {
        var errors = await validator.GetErrorsAsync(request);
        var result = errors.Count > 0 ? Result.Invalid(errors) : await CheckAsync(request, id);

        if (!result.IsSuccess)
            logger.LogWarning("{Entity} kaydı reddedildi: {Errors}", EntityName, result.Errors);
        return result;
    }

    private Result<T> NotFound<T>(int id)
    {
        logger.LogWarning("{Entity} {Id} bulunamadı", EntityName, id);
        return Result<T>.NotFound();
    }
}
