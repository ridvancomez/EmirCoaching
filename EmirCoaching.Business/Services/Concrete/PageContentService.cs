using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class PageContentService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<UpdatePageContentRequest> validator,
    IUploadCleanupService uploadCleanup,
    ILogger<PageContentService> logger) : IPageContentService
{
    private const string PageNotFound = "Sayfa içeriği bulunamadı.";

    public async Task<Result<IReadOnlyList<PageContentResponse>>> GetAllAsync()
    {
        var pages = await unitOfWork.PageContents.GetAllAsync();
        return Result<IReadOnlyList<PageContentResponse>>.Success(
            mapper.Map<List<PageContentResponse>>(pages.OrderBy(p => p.PageKey)));
    }

    public async Task<Result<PageContentResponse>> GetByPageKeyAsync(string pageKey)
    {
        var page = await unitOfWork.PageContents.GetByPageKeyAsync(pageKey);
        if (page is null)
        {
            logger.LogWarning("Sayfa içeriği {PageKey} bulunamadı", pageKey);
            return Result<PageContentResponse>.NotFound(PageNotFound);
        }

        return Result<PageContentResponse>.Success(mapper.Map<PageContentResponse>(page));
    }

    public async Task<Result> UpdateAsync(string pageKey, UpdatePageContentRequest request)
    {
        var errors = await validator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        // Okuma sorgusu takipsizdir; değişiklik Update ile işaretlenir
        var page = await unitOfWork.PageContents.GetByPageKeyAsync(pageKey);
        if (page is null)
            return Result.NotFound(PageNotFound);

        var oldUploads = UploadPaths.Of(page);
        mapper.Map(request, page);
        unitOfWork.PageContents.Update(page);
        await unitOfWork.SaveChangesAsync();
        await uploadCleanup.DeleteUnusedAsync(oldUploads.Except(UploadPaths.Of(page)));

        logger.LogInformation("Sayfa içeriği {PageKey} güncellendi", pageKey);
        return Result.Success();
    }
}
