using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class SocialLinkService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateSocialLinkRequest> createValidator,
    IValidator<UpdateSocialLinkRequest> updateValidator,
    ILogger<SocialLinkService> logger)
    : CrudService<SocialLink, SocialLinkResponse, CreateSocialLinkRequest, UpdateSocialLinkRequest>(
        unitOfWork.SocialLinks, unitOfWork, mapper, createValidator, updateValidator, logger), ISocialLinkService
{
    public async Task<Result<IReadOnlyList<SocialLinkResponse>>> GetActiveAsync() =>
        Result<IReadOnlyList<SocialLinkResponse>>.Success(
            Mapper.Map<List<SocialLinkResponse>>(await UnitOfWork.SocialLinks.GetActiveOrderedAsync()));

    protected override Task<IReadOnlyList<SocialLink>> ListAsync() => UnitOfWork.SocialLinks.GetOrderedAsync();

    protected override async Task<Result> CheckAsync(CreateSocialLinkRequest request, int? id) =>
        await UnitOfWork.SocialLinks.PlatformExistsAsync(request.Platform, id)
            ? Result.Conflict($"{request.Platform} için zaten bir link var.")
            : Result.Success();
}
