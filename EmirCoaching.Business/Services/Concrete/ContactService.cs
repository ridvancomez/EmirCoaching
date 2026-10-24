using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using FluentValidation;

namespace EmirCoaching.Business.Services.Concrete;

public class ContactService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IConsentService consentService,
    IValidator<CreateContactMessageDto> validator) : IContactService
{
    public async Task<Result> SubmitAsync(CreateContactMessageDto dto, RequestInfo requestInfo)
    {
        var errors = await validator.GetErrorsAsync(dto);
        if (dto.PackageId is { } packageId && await unitOfWork.Packages.GetByIdAsync(packageId) is null)
            errors.Add("Seçilen paket bulunamadı.");
        if (errors.Count > 0)
            return Result.Invalid(errors);

        var message = mapper.Map<ContactMessage>(dto);
        message.Status = ContactMessageStatus.Yeni;
        await unitOfWork.ContactMessages.AddAsync(message);

        await consentService.RecordAsync(new ConsentRecord
        {
            ConsentType = ConsentType.Gizlilik,
            IsGranted = true,
            IpAddress = requestInfo.IpAddress,
            UserAgent = requestInfo.UserAgent,
            ContactMessage = message
        });

        // Mesaj ve onay tek SaveChanges = tek transaction
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<PagedResult<ContactMessageDto>> GetPagedAsync(ContactMessageStatus? status, int page, int size)
    {
        var messages = await unitOfWork.ContactMessages.GetPagedAsync(status, page, size);
        return new PagedResult<ContactMessageDto>(
            mapper.Map<List<ContactMessageDto>>(messages.Items), messages.TotalCount, messages.Page, messages.Size);
    }

    public async Task<Result> UpdateStatusAsync(int id, ContactMessageStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
            return Result.Invalid("Geçersiz durum.");

        var message = await unitOfWork.ContactMessages.GetByIdAsync(id);
        if (message is null)
            return Result.NotFound();

        message.Status = newStatus;
        await unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
