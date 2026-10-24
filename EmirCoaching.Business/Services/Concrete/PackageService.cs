using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;

namespace EmirCoaching.Business.Services.Concrete;

public class PackageService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreatePackageDto> createValidator,
    IValidator<UpdatePackageDto> updateValidator) : IPackageService
{
    private const string SlugInUse = "Bu slug başka bir pakette kullanılıyor.";

    public async Task<IEnumerable<PackageDto>> GetListAsync() =>
        mapper.Map<List<PackageDto>>(await unitOfWork.Packages.GetActiveListAsync());

    public async Task<PackageDetailDto?> GetDetailAsync(string slug)
    {
        var package = await unitOfWork.Packages.GetBySlugWithDetailsAsync(slug);
        return package is null ? null : mapper.Map<PackageDetailDto>(package);
    }

    public async Task<Result<int>> CreateAsync(CreatePackageDto dto)
    {
        var errors = await createValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result<int>.Invalid(errors);

        if (await unitOfWork.Packages.SlugExistsAsync(dto.Slug))
            return Result<int>.Conflict(SlugInUse);

        var package = mapper.Map<Package>(dto);
        AddChildren(package, dto);

        await unitOfWork.Packages.AddAsync(package);
        await unitOfWork.SaveChangesAsync();
        return Result<int>.Success(package.Id);
    }

    public async Task<Result> UpdateAsync(UpdatePackageDto dto)
    {
        var errors = await updateValidator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var package = await unitOfWork.Packages.GetByIdWithDetailsAsync(dto.Id);
        if (package is null)
            return Result.NotFound();

        if (await unitOfWork.Packages.SlugExistsAsync(dto.Slug, dto.Id))
            return Result.Conflict(SlugInUse);

        mapper.Map(dto, package);

        // Alt kayıtlar gönderilen listeyle değiştirilir; eskileri soft delete olur.
        // Modül silinince takip edilen kalemleri de cascade ile soft delete olur.
        var moduleRepository = unitOfWork.Repository<PackageModule>();
        foreach (var module in package.Modules.ToList())
            moduleRepository.Remove(module);

        var highlightRepository = unitOfWork.Repository<PackageHighlight>();
        foreach (var highlight in package.Highlights.ToList())
            highlightRepository.Remove(highlight);

        AddChildren(package, dto);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        // Alt kayıtlar yüklü olmalı ki cascade ile birlikte soft delete olsunlar
        var package = await unitOfWork.Packages.GetByIdWithDetailsAsync(id);
        if (package is null)
            return Result.NotFound();

        unitOfWork.Packages.Remove(package);
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> ReorderModulesAsync(int packageId, List<int> moduleIds)
    {
        var package = await unitOfWork.Packages.GetByIdWithDetailsAsync(packageId);
        if (package is null)
            return Result.NotFound();

        var modules = package.Modules.ToDictionary(m => m.Id);
        if (moduleIds.Count != modules.Count || moduleIds.Distinct().Count() != moduleIds.Count
            || !moduleIds.All(modules.ContainsKey))
            return Result.Invalid("Sıralama listesi paketin tüm modüllerini birer kez içermeli.");

        for (var i = 0; i < moduleIds.Count; i++)
            modules[moduleIds[i]].SortOrder = i + 1;

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private static void AddChildren(Package package, CreatePackageDto dto)
    {
        for (var m = 0; m < dto.Modules.Count; m++)
        {
            var input = dto.Modules[m];
            var module = new PackageModule { Title = input.Title, IconKey = input.IconKey, SortOrder = m + 1 };

            for (var i = 0; i < input.Items.Count; i++)
            {
                var item = input.Items[i];
                module.Items.Add(new PackageModuleItem
                {
                    Text = item.Text,
                    State = item.State,
                    LockedLabel = item.LockedLabel,
                    SortOrder = i + 1
                });
            }

            package.Modules.Add(module);
        }

        for (var h = 0; h < dto.Highlights.Count; h++)
            package.Highlights.Add(new PackageHighlight { Text = dto.Highlights[h], SortOrder = h + 1 });
    }
}
