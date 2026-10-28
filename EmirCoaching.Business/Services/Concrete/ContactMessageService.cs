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

public class ContactMessageService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConsentRecordService consentRecordService,
    IValidator<CreateContactMessageRequest> validator,
    ILogger<ContactMessageService> logger) : IContactMessageService
{
    public async Task<Result> SubmitAsync(CreateContactMessageRequest request, RequestInfo requestInfo)
    {
        var errors = await validator.GetErrorsAsync(request);
        if (request.PackageId is { } packageId && await unitOfWork.Packages.GetByIdAsync(packageId) is null)
            errors.Add("Seçilen paket bulunamadı.");
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var message = mapper.Map<ContactMessage>(request);
        message.Status = ContactMessageStatus.Yeni;
        await unitOfWork.ContactMessages.AddAsync(message);

        await consentRecordService.RecordAsync(new ConsentRecord
        {
            ConsentType = ConsentType.Gizlilik,
            IsGranted = true,
            IpAddress = requestInfo.IpAddress,
            UserAgent = requestInfo.UserAgent,
            ContactMessage = message
        });

        // Mesaj ve onay tek SaveChanges = tek transaction
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("İletişim mesajı {Id} alındı", message.Id);
        return Result.Success();
    }

    public async Task<Result<PagedResult<ContactMessageResponse>>> GetPagedAsync(ContactMessageStatus? status, int page, int size)
    {
        var messages = await unitOfWork.ContactMessages.GetPagedAsync(status, page, size);
        return Result<PagedResult<ContactMessageResponse>>.Success(mapper.MapPaged<ContactMessage, ContactMessageResponse>(messages));
    }

    // KVKK silme talebi: kayıt onaylarıyla birlikte kalıcı silinir
    public async Task<Result> DeleteAsync(int id)
    {
        if (!await unitOfWork.DeletePermanentlyAsync(unitOfWork.ContactMessages, id))
            return Result.NotFound("Mesaj bulunamadı.");

        logger.LogInformation("İletişim mesajı {Id} kalıcı olarak silindi", id);
        return Result.Success();
    }

    public async Task<Result> DeleteManyAsync(IReadOnlyCollection<int> ids)
    {
        var check = BulkDelete.Validate(ids);
        if (!check.IsSuccess)
            return check;

        if (!await unitOfWork.DeletePermanentlyAsync(unitOfWork.ContactMessages, ids))
            return Result.NotFound("Seçilen mesajlardan biri bulunamadı.");

        logger.LogInformation("{Count} iletişim mesajı kalıcı olarak silindi: {Ids}", ids.Distinct().Count(), ids.Distinct());
        return Result.Success();
    }

    public async Task<Result> UpdateStatusAsync(int id, ContactMessageStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
            return Result.Invalid("Geçersiz durum.");

        var message = await unitOfWork.ContactMessages.GetByIdAsync(id);
        if (message is null)
            return Result.NotFound("Mesaj bulunamadı.");

        message.Status = newStatus;
        await unitOfWork.SaveChangesAsync();

        logger.LogInformation("İletişim mesajı {Id} durumu {Status} oldu", id, newStatus);
        return Result.Success();
    }
}
