using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;

namespace EmirCoaching.Business.Services.Concrete;

public class ConsentService(IUnitOfWork unitOfWork) : IConsentService
{
    private const int IpMaxLength = 45;
    private const int UserAgentMaxLength = 512;

    // Onay türü → onaylanan yasal metin
    private static readonly Dictionary<ConsentType, LegalDocumentType> DocumentTypes = new()
    {
        [ConsentType.Gizlilik] = LegalDocumentType.Gizlilik,
        [ConsentType.AcikRizaSaglik] = LegalDocumentType.AcikRiza,
        [ConsentType.TicariIleti] = LegalDocumentType.TicariIleti
    };

    public async Task RecordAsync(ConsentRecord record)
    {
        var ownerCount = new object?[]
        {
            record.ContactMessage ?? (object?)record.ContactMessageId,
            record.DietFormSubmission ?? (object?)record.DietFormSubmissionId,
            record.NewsletterSubscriber ?? (object?)record.NewsletterSubscriberId
        }.Count(o => o is not null);

        if (ownerCount != 1)
            throw new InvalidOperationException("Onay kaydının tam bir sahibi olmalı (iletişim, diyet formu veya bülten).");

        // Onay anındaki güncel metin sürümüne bağlanır
        if (record.LegalDocumentId is null && record.LegalDocument is null)
            record.LegalDocumentId = (await unitOfWork.LegalDocuments.GetCurrentAsync(DocumentTypes[record.ConsentType]))?.Id;

        if (record.GrantedAt == default)
            record.GrantedAt = DateTime.UtcNow;

        record.IpAddress = Truncate(record.IpAddress, IpMaxLength);
        record.UserAgent = Truncate(record.UserAgent, UserAgentMaxLength);

        await unitOfWork.Repository<ConsentRecord>().AddAsync(record);
    }

    private static string Truncate(string? value, int maxLength) =>
        string.IsNullOrEmpty(value) ? string.Empty : value.Length <= maxLength ? value : value[..maxLength];
}
