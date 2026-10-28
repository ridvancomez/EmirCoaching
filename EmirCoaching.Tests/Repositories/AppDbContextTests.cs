using AwesomeAssertions;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EmirCoaching.Tests.Repositories;

public class AppDbContextTests : IDisposable
{
    private readonly SqliteDatabase _db = new();

    public void Dispose() => _db.Dispose();

    private async Task<int> AddCategoryAsync(string slug = "test-kategori")
    {
        await using var context = _db.CreateContext();
        var category = new BlogCategory { Name = "Test", Slug = slug };
        context.BlogCategories.Add(category);
        await context.SaveChangesAsync();
        return category.Id;
    }

    [Fact]
    public async Task SaveChanges_OnAdd_SetsCreatedAt()
    {
        // Act
        var id = await AddCategoryAsync();

        // Assert
        await using var context = _db.CreateContext();
        var category = await context.BlogCategories.SingleAsync(c => c.Id == id);
        category.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        category.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public async Task SaveChanges_OnModify_SetsUpdatedAt()
    {
        // Arrange
        var id = await AddCategoryAsync();

        // Act
        await using (var context = _db.CreateContext())
        {
            var category = await context.BlogCategories.SingleAsync(c => c.Id == id);
            category.Name = "Değişti";
            await context.SaveChangesAsync();
        }

        // Assert
        await using var check = _db.CreateContext();
        (await check.BlogCategories.SingleAsync(c => c.Id == id)).UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task SaveChanges_OnRemove_SoftDeletesInsteadOfDeleting()
    {
        // Arrange
        var id = await AddCategoryAsync();

        // Act
        await using (var context = _db.CreateContext())
        {
            context.BlogCategories.Remove(await context.BlogCategories.SingleAsync(c => c.Id == id));
            await context.SaveChangesAsync();
        }

        // Assert — satır duruyor, işaretlendi
        await using var check = _db.CreateContext();
        var row = await check.BlogCategories.IgnoreQueryFilters().SingleAsync(c => c.Id == id);
        row.IsDeleted.Should().BeTrue();
        row.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task QueryFilter_HidesSoftDeletedRecords()
    {
        // Arrange
        var id = await AddCategoryAsync();
        await using (var context = _db.CreateContext())
        {
            context.BlogCategories.Remove(await context.BlogCategories.SingleAsync(c => c.Id == id));
            await context.SaveChangesAsync();
        }

        // Act
        await using var check = _db.CreateContext();
        var found = await check.BlogCategories.AnyAsync(c => c.Id == id);

        // Assert
        found.Should().BeFalse();
    }

    [Fact]
    public async Task UniqueSlugIndex_AllowsReusingSlugOfDeletedRecord()
    {
        // Arrange — filtreli unique index ([IsDeleted] = 0): silinen kaydın slug'ı serbest kalır
        var id = await AddCategoryAsync("tekrar");
        await using (var context = _db.CreateContext())
        {
            context.BlogCategories.Remove(await context.BlogCategories.SingleAsync(c => c.Id == id));
            await context.SaveChangesAsync();
        }

        // Act
        var act = () => AddCategoryAsync("tekrar");

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task UniqueSlugIndex_RejectsDuplicateActiveSlug()
    {
        // Arrange
        await AddCategoryAsync("ayni");

        // Act
        var act = () => AddCategoryAsync("ayni");

        // Assert — servisteki SlugExistsAsync kontrolü atlansa bile veritabanı engeller
        await act.Should().ThrowAsync<DbUpdateException>();
    }
}
