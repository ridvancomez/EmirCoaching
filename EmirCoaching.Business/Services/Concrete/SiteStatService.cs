using AutoMapper;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class SiteStatService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateSiteStatRequest> createValidator,
    IValidator<UpdateSiteStatRequest> updateValidator,
    ILogger<SiteStatService> logger)
    : CrudService<SiteStat, SiteStatResponse, CreateSiteStatRequest, UpdateSiteStatRequest>(
        unitOfWork.SiteStats, unitOfWork, mapper, createValidator, updateValidator, logger), ISiteStatService
{
    protected override Task<IReadOnlyList<SiteStat>> ListAsync() => UnitOfWork.SiteStats.GetOrderedAsync();
}
