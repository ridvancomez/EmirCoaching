using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class CertificateService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IValidator<CreateCertificateRequest> createValidator,
    IValidator<UpdateCertificateRequest> updateValidator,
    IUploadCleanupService uploadCleanup,
    ILogger<CertificateService> logger)
    : CrudService<Certificate, CertificateResponse, CreateCertificateRequest, UpdateCertificateRequest>(
        unitOfWork.Certificates, unitOfWork, mapper, createValidator, updateValidator, logger), ICertificateService
{
    public async Task<Result<IReadOnlyList<CertificateResponse>>> GetActiveAsync() =>
        Result<IReadOnlyList<CertificateResponse>>.Success(
            Mapper.Map<List<CertificateResponse>>(await UnitOfWork.Certificates.GetActiveOrderedAsync()));

    protected override Task<IReadOnlyList<Certificate>> ListAsync() => UnitOfWork.Certificates.GetOrderedAsync();

    protected override Task ReleaseUploadsAsync(IEnumerable<string> paths) => uploadCleanup.DeleteUnusedAsync(paths);
}
