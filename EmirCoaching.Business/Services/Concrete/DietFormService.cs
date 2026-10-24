using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using FluentValidation;

namespace EmirCoaching.Business.Services.Concrete;

public class DietFormService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConsentService consentService,
    IValidator<CreateDietFormDto> validator) : IDietFormService
{
    public async Task<Result> SubmitAsync(CreateDietFormDto dto, RequestInfo requestInfo)
    {
        var errors = await validator.GetErrorsAsync(dto);
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var submission = mapper.Map<DietFormSubmission>(dto);
        submission.Status = DietFormStatus.Yeni;
        await unitOfWork.DietForms.AddAsync(submission);

        await RecordConsentAsync(ConsentType.Gizlilik, submission, requestInfo);
        if (dto.HealthDataConsent)
            await RecordConsentAsync(ConsentType.AcikRizaSaglik, submission, requestInfo);

        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<PagedResult<DietFormDto>> GetPagedAsync(DietFormStatus? status, int page, int size)
    {
        var submissions = await unitOfWork.DietForms.GetPagedAsync(status, page, size);
        return new PagedResult<DietFormDto>(
            mapper.Map<List<DietFormDto>>(submissions.Items), submissions.TotalCount, submissions.Page, submissions.Size);
    }

    public async Task<Result> UpdateStatusAsync(int id, DietFormStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
            return Result.Invalid("Geçersiz durum.");

        var submission = await unitOfWork.DietForms.GetByIdAsync(id);
        if (submission is null)
            return Result.NotFound();

        submission.Status = newStatus;
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private Task RecordConsentAsync(ConsentType type, DietFormSubmission submission, RequestInfo requestInfo) =>
        consentService.RecordAsync(new ConsentRecord
        {
            ConsentType = type,
            IsGranted = true,
            IpAddress = requestInfo.IpAddress,
            UserAgent = requestInfo.UserAgent,
            DietFormSubmission = submission
        });
}
