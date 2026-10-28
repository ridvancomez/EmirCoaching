using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.Tests.Repositories;

// KVKK: form kayıtları soft delete'e dönüşmeden, onay kayıtlarıyla birlikte veritabanından silinir.
// Üç form repository'si aynı tabanı (PersonalDataRepository) kullanır; bülten üzerinden sınanır.
public class PersonalDataRepositoryTests : IDisposable
{
    private readonly SqliteDatabase _db = new();

    public void Dispose() => _db.Dispose();

    private async Task<int> AddSubscriberWithConsentAsync(string email)
    {
        await using var context = _db.CreateContext();
        var subscriber = new NewsletterSubscriber { Email = email, IpAddress = "10.0.0.1", IsActive = true };
        subscriber.ConsentRecords.Add(new ConsentRecord { ConsentType = ConsentType.Gizlilik, IsGranted = true, GrantedAt = DateTime.UtcNow });
        context.NewsletterSubscribers.Add(subscriber);
        await context.SaveChangesAsync();
        return subscriber.Id;
    }

    [Fact]
    public async Task DeletePermanentlyAsync_RemovesRecordAndItsConsentsFromDatabase()
    {
        // Arrange
        var id = await AddSubscriberWithConsentAsync("ali@example.com");

        // Act
        bool deleted;
        await using (var context = _db.CreateContext())
            deleted = await new NewsletterSubscriberRepository(context).DeletePermanentlyAsync(id);

        // Assert — IgnoreQueryFilters ile bile bulunmaz: soft delete değil
        await using var check = _db.CreateContext();
        deleted.Should().BeTrue();
        (await check.NewsletterSubscribers.IgnoreQueryFilters().AnyAsync(s => s.Id == id)).Should().BeFalse();
        (await check.ConsentRecords.IgnoreQueryFilters().AnyAsync(c => c.NewsletterSubscriberId == id)).Should().BeFalse();
    }

    [Fact]
    public async Task DeletePermanentlyAsync_KeepsOtherRecordsConsents()
    {
        // Arrange
        var id = await AddSubscriberWithConsentAsync("ali@example.com");
        var otherId = await AddSubscriberWithConsentAsync("ayse@example.com");

        // Act
        await using (var context = _db.CreateContext())
            await new NewsletterSubscriberRepository(context).DeletePermanentlyAsync(id);

        // Assert
        await using var check = _db.CreateContext();
        (await check.ConsentRecords.CountAsync(c => c.NewsletterSubscriberId == otherId)).Should().Be(1);
    }

    [Fact]
    public async Task DeletePermanentlyAsync_WithSoftDeletedRecord_StillDeletesIt()
    {
        // Arrange
        var id = await AddSubscriberWithConsentAsync("ali@example.com");
        await using (var context = _db.CreateContext())
        {
            context.NewsletterSubscribers.Remove(await context.NewsletterSubscribers.SingleAsync(s => s.Id == id));
            await context.SaveChangesAsync();
        }

        // Act
        bool deleted;
        await using (var context = _db.CreateContext())
            deleted = await new NewsletterSubscriberRepository(context).DeletePermanentlyAsync(id);

        // Assert
        deleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeletePermanentlyAsync_WithMissingId_ReturnsFalse()
    {
        // Act
        await using var context = _db.CreateContext();
        var deleted = await new NewsletterSubscriberRepository(context).DeletePermanentlyAsync(999);

        // Assert
        deleted.Should().BeFalse();
    }

    [Fact]
    public async Task DeletePermanentlyAsync_ManyWithOneMissing_RestoresAlreadyDeletedRecord()
    {
        // Arrange
        var id = await AddSubscriberWithConsentAsync("ali@example.com");

        // Act — ilk kayıt silinir, 999 yok: transaction geri alınmalı
        bool deleted;
        await using (var context = _db.CreateContext())
        using (var unitOfWork = new UnitOfWork(context))
            deleted = await unitOfWork.DeletePermanentlyAsync(unitOfWork.NewsletterSubscribers, [id, 999]);

        // Assert
        await using var check = _db.CreateContext();
        deleted.Should().BeFalse();
        (await check.NewsletterSubscribers.AnyAsync(s => s.Id == id)).Should().BeTrue();
        (await check.ConsentRecords.AnyAsync(c => c.NewsletterSubscriberId == id)).Should().BeTrue();
    }

    [Fact]
    public async Task CountByIpSinceAsync_CountsOnlyRecentSubscriptionsFromSameIp()
    {
        // Arrange
        await using (var context = _db.CreateContext())
        {
            context.NewsletterSubscribers.AddRange(
                new NewsletterSubscriber { Email = "a@example.com", IpAddress = "10.0.0.1" },
                new NewsletterSubscriber { Email = "b@example.com", IpAddress = "10.0.0.1", CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new NewsletterSubscriber { Email = "c@example.com", IpAddress = "10.0.0.2" });
            await context.SaveChangesAsync();
        }

        // Act
        await using var check = _db.CreateContext();
        var count = await new NewsletterSubscriberRepository(check).CountByIpSinceAsync("10.0.0.1", DateTime.UtcNow.AddHours(-24));

        // Assert
        count.Should().Be(1);
    }
}
