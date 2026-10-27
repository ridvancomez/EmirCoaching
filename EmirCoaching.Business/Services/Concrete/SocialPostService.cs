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

// Elle girilen postlar (YouTube). Instagram senkronu ayrı bir iş olarak eklenecek.
public class SocialPostService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateSocialPostRequest> createValidator,
    IValidator<UpdateSocialPostRequest> updateValidator,
    IUploadCleanupService uploadCleanup,
    ILogger<SocialPostService> logger)
    : CrudService<SocialPost, SocialPostResponse, CreateSocialPostRequest, UpdateSocialPostRequest>(
        unitOfWork.SocialPosts, unitOfWork, mapper, createValidator, updateValidator, logger), ISocialPostService
{
    private const int MaxCount = 12;

    public async Task<Result<IReadOnlyList<SocialPostResponse>>> GetActiveByPlatformAsync(SocialPlatform platform, int count)
    {
        var posts = await UnitOfWork.SocialPosts.GetActiveByPlatformAsync(platform, Math.Clamp(count, 1, MaxCount));
        return Result<IReadOnlyList<SocialPostResponse>>.Success(Mapper.Map<List<SocialPostResponse>>(posts));
    }

    protected override Task<IReadOnlyList<SocialPost>> ListAsync() => UnitOfWork.SocialPosts.GetOrderedAsync();

    protected override Task ReleaseUploadsAsync(IEnumerable<string> paths) => uploadCleanup.DeleteUnusedAsync(paths);
}
