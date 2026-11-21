using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Mapping;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace EmirCoaching.Business.Services.Concrete;

public class DietFormSubmissionService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConsentRecordService consentRecordService,
    IValidator<CreateDietFormSubmissionRequest> validator,
    IValidator<UpdateDietFormSubmissionClientFormRequest> clientFormValidator,
    ILogger<DietFormSubmissionService> logger) : IDietFormSubmissionService
{
    public async Task<Result> SubmitAsync(CreateDietFormSubmissionRequest request, RequestInfo requestInfo)
    {
        var errors = await validator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        // Satıştan kaldırılmış (pasif) paket seçilemez
        var package = await unitOfWork.Packages.GetByIdAsync(request.PackageId);
        if (package is not { IsActive: true })
        {
            logger.LogWarning("Diyet formu reddedildi: paket {PackageId} bulunamadı", request.PackageId);
            return Result.Invalid("Seçilen paket bulunamadı.");
        }

        var submission = mapper.Map<DietFormSubmission>(request);
        submission.Email = request.Email.Trim().ToLowerInvariant();
        submission.PhoneNumber = PhoneHelper.Normalize(request.PhoneNumber);

        // Değerlendirilmeyi bekleyen başvurusu olan kişi yeniden gönderemez; admin durumu değiştirince açılır
        if (await unitOfWork.DietFormSubmissions.HasPendingAsync(submission.Email, submission.PhoneNumber))
        {
            logger.LogWarning("Diyet formu reddedildi: bekleyen başvuru var");
            return Result.Conflict("Başvurunuz bize ulaştı. İnceleyip en kısa sürede size dönüş sağlayacağız.");
        }

        submission.Status = DietFormStatus.Yeni;
        await unitOfWork.DietFormSubmissions.AddAsync(submission);

        await RecordConsentAsync(ConsentType.Gizlilik, submission, requestInfo);
        if (request.HealthDataConsent)
            await RecordConsentAsync(ConsentType.AcikRizaSaglik, submission, requestInfo);

        await unitOfWork.SaveChangesAsync();

        // Sağlık verisi içerdiği için loga kişisel bilgi yazılmaz, yalnızca kayıt numarası
        logger.LogInformation("Diyet formu {Id} alındı", submission.Id);
        return Result.Success();
    }

    public async Task<Result<PagedResult<DietFormSubmissionResponse>>> GetPagedAsync(DietFormStatus? status, int page, int size)
    {
        var submissions = await unitOfWork.DietFormSubmissions.GetPagedAsync(status, page, size);
        return Result<PagedResult<DietFormSubmissionResponse>>.Success(
            mapper.MapPaged<DietFormSubmission, DietFormSubmissionResponse>(submissions));
    }

    // KVKK silme talebi: kayıt onaylarıyla birlikte kalıcı silinir
    public async Task<Result> DeleteAsync(int id)
    {
        if (!await unitOfWork.DeletePermanentlyAsync(unitOfWork.DietFormSubmissions, id))
            return Result.NotFound("Başvuru bulunamadı.");

        logger.LogInformation("Diyet formu {Id} kalıcı olarak silindi", id);
        return Result.Success();
    }

    public async Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids)
    {
        var check = BulkDelete.Validate(ids);
        if (!check.IsSuccess)
            return check;

        if (!await unitOfWork.DeletePermanentlyAsync(unitOfWork.DietFormSubmissions, ids))
            return Result.NotFound("Seçilen başvurulardan biri bulunamadı.");

        logger.LogInformation("{Count} diyet formu kalıcı olarak silindi: {Ids}", ids.Distinct().Count(), ids.Distinct());
        return Result.Success();
    }

    public async Task<Result> UpdateStatusAsync(int id, DietFormStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
            return Result.Invalid("Geçersiz durum.");

        var submission = await unitOfWork.DietFormSubmissions.GetByIdAsync(id);
        if (submission is null)
            return Result.NotFound("Başvuru bulunamadı.");

        submission.Status = newStatus;
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Diyet formu {Id} durumu {Status} oldu", id, newStatus);
        return Result.Success();
    }

    public async Task<Result> UpdateClientFormAsync(int id, UpdateDietFormSubmissionClientFormRequest request)
    {
        var errors = await clientFormValidator.GetErrorsAsync(request);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var submission = await unitOfWork.DietFormSubmissions.GetByIdAsync(id);
        if (submission is null)
            return Result.NotFound("Başvuru bulunamadı.");

        submission.ClientFormUrl = string.IsNullOrWhiteSpace(request.ClientFormUrl) ? null : request.ClientFormUrl.Trim();
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("Diyet formu {Id} Google Form linki {Action}", id, submission.ClientFormUrl is null ? "silindi" : "kaydedildi");
        return Result.Success();
    }

    private Task RecordConsentAsync(ConsentType type, DietFormSubmission submission, RequestInfo requestInfo) =>
        consentRecordService.RecordAsync(new ConsentRecord
        {
            ConsentType = type,
            IsGranted = true,
            IpAddress = requestInfo.IpAddress,
            UserAgent = requestInfo.UserAgent,
            DietFormSubmission = submission
        });
}
