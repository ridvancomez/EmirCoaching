using AwesomeAssertions;
using EmirCoaching.DataAccess.Repositories.Concrete;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;

namespace EmirCoaching.Tests.Repositories;

// Ana sayfa carousel'i: yalnızca aktif + ShowOnHome yorumlar, paketi pasif olan çıkmaz
public class TestimonialRepositoryTests : IDisposable
{
    private readonly SqliteDatabase _db = new();

    public void Dispose() => _db.Dispose();

    private async Task<int> AddPackageAsync(string slug, bool isActive)
    {
        await using var context = _db.CreateContext();
        var package = new Package
        {
            Name = slug, Slug = slug, Subtitle = "Alt", DurationMonths = 6, Price = 1000, Location = PackageLocation.Evde,
            ShortDescription = "Kısa", PurchaseUrl = "https://example.com", IsActive = isActive
        };
        context.Packages.Add(package);
        await context.SaveChangesAsync();
        return package.Id;
    }

    private async Task AddTestimonialAsync(int packageId, string name, bool isActive = true, bool showOnHome = true, int sortOrder = 0)
    {
        await using var context = _db.CreateContext();
        context.Testimonials.Add(new Testimonial
        {
            PackageId = packageId, Name = name, Text = "Metin", Rating = 4, // SQLite decimal'ı metin saklar: 5.0 > "5" sayılıp CK_Testimonials_Rating'e takılır
            IsActive = isActive, ShowOnHome = showOnHome, SortOrder = sortOrder
        });
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetHomeAsync_ReturnsOnlyActiveHomeTestimonialsOfActivePackages()
    {
        // Arrange
        var active = await AddPackageAsync("aktif", isActive: true);
        var inactive = await AddPackageAsync("pasif", isActive: false);
        await AddTestimonialAsync(active, "Gösterilir");
        await AddTestimonialAsync(active, "Ana sayfada değil", showOnHome: false);
        await AddTestimonialAsync(active, "Pasif yorum", isActive: false);
        await AddTestimonialAsync(inactive, "Pasif paket");

        // Act
        await using var context = _db.CreateContext();
        var home = await new TestimonialRepository(context).GetHomeAsync();

        // Assert
        home.Select(t => t.Name).Should().Equal("Gösterilir");
    }

    [Fact]
    public async Task GetHomeAsync_LoadsPackageInSortOrder()
    {
        // Arrange
        var packageId = await AddPackageAsync("aktif", isActive: true);
        await AddTestimonialAsync(packageId, "İkinci", sortOrder: 2);
        await AddTestimonialAsync(packageId, "Birinci", sortOrder: 1);

        // Act
        await using var context = _db.CreateContext();
        var home = await new TestimonialRepository(context).GetHomeAsync();

        // Assert
        home.Select(t => t.Name).Should().Equal("Birinci", "İkinci");
        home.Should().OnlyContain(t => t.Package.Slug == "aktif");
    }
}
