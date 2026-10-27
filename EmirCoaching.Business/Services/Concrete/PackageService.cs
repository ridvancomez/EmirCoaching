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

public class PackageService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreatePackageRequest> createValidator,
    IValidator<UpdatePackageRequest> updateValidator,
    ILogger<PackageService> logger) : IPackageService
{
    private const string SlugInUse = "Bu slug başka bir pakette kullanılıyor.";

    public async Task<Result<IReadOnlyList<PackageResponse>>> GetListAsync() =>
        Result<IReadOnlyList<PackageResponse>>.Success(
            mapper.Map<List<PackageResponse>>(await unitOfWork.Packages.GetActiveListAsync()));

    public async Task<Result<PackageDetailResponse>> GetDetailAsync(string slug)
    {
        var package = await unitOfWork.Packages.GetBySlugWithDetailsAsync(slug);
        if (package is null)
        {
            logger.LogWarning("Paket {Slug} bulunamadı", slug);
            return Result<PackageDetailResponse>.NotFound("Paket bulunamadı.");
        }

        return Result<PackageDetailResponse>.Success(mapper.Map<PackageDetailResponse>(package));
    }

    public async Task<Result<IReadOnlyList<PackageResponse>>> GetAllAsync() =>
        Result<IReadOnlyList<PackageResponse>>.Success(
            mapper.Map<List<PackageResponse>>(await unitOfWork.Packages.GetOrderedListAsync()));

    public async Task<Result<PackageDetailResponse>> GetByIdAsync(int id)
    {
        var package = await unitOfWork.Packages.GetDetailByIdAsync(id);
        if (package is null)
        {
            logger.LogWarning("Paket {Id} bulunamadı", id);
            return Result<PackageDetailResponse>.NotFound("Paket bulunamadı.");
        }

        return Result<PackageDetailResponse>.Success(mapper.Map<PackageDetailResponse>(package));
    }

    public async Task<Result<int>> CreateAsync(CreatePackageRequest request)
    {
        var errors = await createValidator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        if (await unitOfWork.Packages.SlugExistsAsync(request.Slug))
            return Result<int>.Conflict(SlugInUse);

        var package = mapper.Map<Package>(request);
        SyncChildren(package, request);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            await ReleaseFeaturedAsync(request.Location, request.IsFeatured, currentId: null);
            await unitOfWork.Packages.AddAsync(package);
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }

        logger.LogInformation("Paket {Id} oluşturuldu", package.Id);
        return Result<int>.Success(package.Id);
    }

    public async Task<Result> UpdateAsync(UpdatePackageRequest request)
    {
        var errors = await updateValidator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var package = await unitOfWork.Packages.GetByIdWithDetailsAsync(request.Id);
        if (package is null)
            return Result.NotFound("Paket bulunamadı.");

        var childErrors = CheckChildIds(package, request);
        if (childErrors.Count > 0)
            return Result.Invalid(childErrors);

        if (await unitOfWork.Packages.SlugExistsAsync(request.Slug, request.Id))
            return Result.Conflict(SlugInUse);

        await unitOfWork.BeginTransactionAsync();
        try
        {
            // Paket değişmeden önce çalışmalı: aksi halde eski ve yeni öne çıkan aynı SaveChanges'e düşer
            await ReleaseFeaturedAsync(request.Location, request.IsFeatured, request.Id);

            mapper.Map(request, package);
            SyncChildren(package, request);

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }

        logger.LogInformation("Paket {Id} güncellendi", package.Id);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        // Alt kayıtlar (modül, kalem, öne çıkan, yorum) yüklü olmalı ki cascade ile birlikte soft delete olsunlar
        var package = await unitOfWork.Packages.GetByIdWithDetailsAsync(id);
        if (package is null)
            return Result.NotFound("Paket bulunamadı.");

        unitOfWork.Packages.Remove(package);
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Paket {Id} silindi", id);
        return Result.Success();
    }

    public async Task<Result> ReorderModulesAsync(int packageId, List<int> moduleIds)
    {
        var package = await unitOfWork.Packages.GetByIdWithDetailsAsync(packageId);
        if (package is null)
            return Result.NotFound("Paket bulunamadı.");

        var modules = package.Modules.ToDictionary(m => m.Id);
        if (moduleIds.Count != modules.Count || moduleIds.Distinct().Count() != moduleIds.Count
            || !moduleIds.All(modules.ContainsKey))
            return Result.Invalid("Sıralama listesi paketin tüm modüllerini birer kez içermeli.");

        for (var i = 0; i < moduleIds.Count; i++)
            modules[moduleIds[i]].SortOrder = i + 1;

        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Paket {Id} modül sırası güncellendi", packageId);
        return Result.Success();
    }

    // "En Çok Tercih Edilen" her kategoride tektir: paket işaretlendiyse aynı kategorideki eskisi kaldırılır.
    // Unique index ihlal edilmesin diye eskisi önce ayrı SaveChanges ile kaydedilir (çağıran transaction açar).
    private async Task ReleaseFeaturedAsync(PackageLocation location, bool isFeatured, int? currentId)
    {
        if (!isFeatured)
            return;

        var previous = await unitOfWork.Packages.GetFeaturedAsync(location);
        if (previous is null || previous.Id == currentId)
            return;

        previous.IsFeatured = false;
        await unitOfWork.SaveChangesAsync();
    }

    // Gönderilen alt kayıt Id'leri bu pakete (kalemde ise aynı modüle) ait olmalı ve tekrar etmemeli.
    private static List<string> CheckChildIds(Package package, UpdatePackageRequest request)
    {
        var errors = new List<string>();
        var modules = package.Modules.ToDictionary(m => m.Id);

        if (!ChildListSync.AreKnownIds(request.Modules.Select(m => m.Id), modules.Keys))
            errors.Add("Gönderilen modüllerden biri bu pakete ait değil veya tekrar ediyor.");
        if (!ChildListSync.AreKnownIds(request.Highlights.Select(h => h.Id), package.Highlights.Select(h => h.Id)))
            errors.Add("Gönderilen öne çıkan maddelerden biri bu pakete ait değil veya tekrar ediyor.");

        var itemsValid = request.Modules.All(m =>
        {
            var known = m.Id is { } id && modules.TryGetValue(id, out var module) ? module.Items.Select(i => i.Id) : [];
            return ChildListSync.AreKnownIds(m.Items.Select(i => i.Id), known);
        });
        if (!itemsValid)
            errors.Add("Gönderilen kalemlerden biri kendi modülüne ait değil veya tekrar ediyor.");

        return errors;
    }

    // Alt kayıtlar gönderilen listeyle eşitlenir; numaraları korunur. Listeden çıkan kayıt soft delete olur
    // (modülle birlikte kalemleri de cascade ile). Liste sırası SortOrder olur.
    private void SyncChildren(Package package, CreatePackageRequest request)
    {
        ChildListSync.Sync(package.Modules, request.Modules, m => m.Id, unitOfWork.PackageModules.Remove, (module, input, order) =>
        {
            module.Title = input.Title;
            module.IconKey = input.IconKey;
            module.SortOrder = order;
            ChildListSync.Sync(module.Items, input.Items, i => i.Id, item => module.Items.Remove(item), (item, itemInput, itemOrder) =>
            {
                item.Text = itemInput.Text;
                item.IsLocked = itemInput.IsLocked;
                item.SortOrder = itemOrder;
            });
        });

        ChildListSync.Sync(package.Highlights, request.Highlights, h => h.Id, unitOfWork.PackageHighlights.Remove, (highlight, input, order) =>
        {
            highlight.Text = input.Text;
            highlight.IsLocked = input.IsLocked;
            highlight.SortOrder = order;
        });
    }
}
