using AwesomeAssertions;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;

namespace EmirCoaching.Tests.Repositories;

public class PackageRepositoryTests : IDisposable
{
    private readonly SqliteDatabase _db = new();

    public void Dispose() => _db.Dispose();

    private async Task<Package> AddPackageAsync(string slug, bool isActive = true, bool isFeatured = false)
    {
        await using var context = _db.CreateContext();
        var package = new Package
        {
            Name = slug, Slug = slug, Subtitle = "Alt", DurationMonths = 6, Price = 1000, Location = PackageLocation.Evde,
            ShortDescription = "Kısa", PurchaseUrl = "https://example.com", IsActive = isActive, IsFeatured = isFeatured,
            Modules =
            [
                new PackageModule { Title = "İkinci", IconKey = "x", SortOrder = 2, Items = [new PackageModuleItem { Text = "b", SortOrder = 2 }, new PackageModuleItem { Text = "a", SortOrder = 1 }] },
                new PackageModule { Title = "Birinci", IconKey = "x", SortOrder = 1 }
            ]
        };
        context.Packages.Add(package);
        await context.SaveChangesAsync();
        return package;
    }

    [Fact]
    public async Task GetDetailByIdAsync_ReturnsModulesAndItemsInSortOrder()
    {
        // Arrange
        var package = await AddPackageAsync("test-paket");

        // Act
        await using var context = _db.CreateContext();
        var detail = await new PackageRepository(context).GetDetailByIdAsync(package.Id);

        // Assert
        detail!.Modules.Select(m => m.Title).Should().Equal("Birinci", "İkinci");
        detail.Modules.Last().Items.Select(i => i.Text).Should().Equal("a", "b");
    }

    [Fact]
    public async Task GetBySlugWithDetailsAsync_WithInactivePackage_ReturnsNull()
    {
        // Arrange — pasif paketin site detayı 404'tür
        await AddPackageAsync("pasif-paket", isActive: false);

        // Act
        await using var context = _db.CreateContext();
        var package = await new PackageRepository(context).GetBySlugWithDetailsAsync("pasif-paket");

        // Assert
        package.Should().BeNull();
    }

    [Fact]
    public async Task SlugExistsAsync_ExcludesGivenId()
    {
        // Arrange
        var package = await AddPackageAsync("benim-slugim");

        // Act
        await using var context = _db.CreateContext();
        var repository = new PackageRepository(context);

        // Assert — kendi slug'ı güncellemede çakışma sayılmaz, başkası için sayılır
        (await repository.SlugExistsAsync("benim-slugim", package.Id)).Should().BeFalse();
        (await repository.SlugExistsAsync("benim-slugim")).Should().BeTrue();
    }

    [Fact]
    public async Task GetActiveListAsync_ExcludesInactivePackages()
    {
        // Arrange
        await AddPackageAsync("aktif-test");
        await AddPackageAsync("pasif-test", isActive: false);

        // Act
        await using var context = _db.CreateContext();
        var slugs = (await new PackageRepository(context).GetActiveListAsync()).Select(p => p.Slug).ToList();

        // Assert
        slugs.Should().Contain("aktif-test");
        slugs.Should().NotContain("pasif-test");
    }
}
