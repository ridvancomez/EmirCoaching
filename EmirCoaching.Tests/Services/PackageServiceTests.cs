using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class PackageServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IPackageRepository> _packages = new();
    private readonly Mock<IPackageModuleRepository> _modules = new();
    private readonly Mock<IPackageHighlightRepository> _highlights = new();
    private readonly PackageService _service;

    public PackageServiceTests()
    {
        _uow.Setup(u => u.Packages).Returns(_packages.Object);
        _uow.Setup(u => u.PackageModules).Returns(_modules.Object);
        _uow.Setup(u => u.PackageHighlights).Returns(_highlights.Object);

        _service = new PackageService(_uow.Object, TestMapper.Create(),
            new CreatePackageRequestValidator(), new UpdatePackageRequestValidator(),
            NullLogger<PackageService>.Instance);
    }

    // ---- Okuma ----

    [Fact]
    public async Task GetDetailAsync_WithMissingSlug_ReturnsNotFound()
    {
        // Act
        var result = await _service.GetDetailAsync("yok");

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnsPackageWithModules()
    {
        // Arrange — sıralama repository'de (Include OrderBy); servis gelen sırayı korur
        _packages.Setup(r => r.GetDetailByIdAsync(1)).ReturnsAsync(ExistingPackage());

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Modules.Select(m => m.Id).Should().Equal(10, 20);
    }

    // ---- CreateAsync ----

    [Fact]
    public async Task CreateAsync_WithValidRequest_AddsPackageInTransaction()
    {
        // Arrange
        _packages.Setup(r => r.AddAsync(It.IsAny<Package>()))
            .Callback<Package>(p => p.Id = 5)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(ValidCreateRequest());

        // Assert
        result.Value.Should().Be(5);
        _packages.Verify(r => r.AddAsync(It.Is<Package>(p => p.Modules.Count == 1 && p.Highlights.Count == 1)), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
        _uow.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_AssignsSortOrderFromListPosition()
    {
        // Arrange
        Package? added = null;
        _packages.Setup(r => r.AddAsync(It.IsAny<Package>())).Callback<Package>(p => added = p);
        var request = ValidCreateRequest() with
        {
            Modules = [Module(null, "Antrenman"), Module(null, "Beslenme")]
        };

        // Act
        await _service.CreateAsync(request);

        // Assert
        added!.Modules.Select(m => (m.Title, m.SortOrder)).Should().Equal(("Antrenman", 1), ("Beslenme", 2));
    }

    [Fact]
    public async Task CreateAsync_WithInvalidRequest_ReturnsValidationErrorAndDoesNotSave()
    {
        // Act
        var result = await _service.CreateAsync(ValidCreateRequest() with { Price = 0 });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithExistingSlug_ReturnsConflict()
    {
        // Arrange
        _packages.Setup(r => r.SlugExistsAsync("baslangic", null)).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(ValidCreateRequest());

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        _packages.Verify(r => r.AddAsync(It.IsAny<Package>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WhenFeatured_ReleasesPreviousFeaturedInSameLocation()
    {
        // Arrange — aynı kategoride (Salonda) zaten öne çıkan bir paket var
        var previous = new Package { Id = 3, IsFeatured = true, Location = PackageLocation.Salonda };
        _packages.Setup(r => r.GetFeaturedAsync(PackageLocation.Salonda)).ReturnsAsync(previous);

        // Act
        await _service.CreateAsync(ValidCreateRequest() with { IsFeatured = true });

        // Assert — eskisi kaldırılıp ayrı kaydedildi (unique index), sonra yeni paket kaydedildi
        previous.IsFeatured.Should().BeFalse();
        _uow.Verify(u => u.SaveChangesAsync(), Times.Exactly(2));
    }

    [Fact]
    public async Task CreateAsync_WhenSaveFails_RollsBackAndRethrows()
    {
        // Arrange
        _uow.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new InvalidOperationException());

        // Act
        var act = () => _service.CreateAsync(ValidCreateRequest());

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _uow.Verify(u => u.RollbackAsync(), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Never);
    }

    // ---- UpdateAsync ----

    [Fact]
    public async Task UpdateAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateAsync(ValidUpdateRequest());

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    [Fact]
    public async Task UpdateAsync_SyncsModulesById()
    {
        // Arrange — 10 güncellenir, 20 listede yok (silinir), Id'siz olan eklenir
        var package = ExistingPackage();
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(package);
        var request = ValidUpdateRequest() with { Modules = [Module(10, "Güncel"), Module(null, "Yeni")] };

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        package.Modules.Should().Contain(m => m.Id == 10 && m.Title == "Güncel" && m.SortOrder == 1);
        package.Modules.Should().Contain(m => m.Id == 0 && m.Title == "Yeni" && m.SortOrder == 2);
        _modules.Verify(r => r.Remove(It.Is<PackageModule>(m => m.Id == 20)), Times.Once);
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithModuleOfAnotherPackage_ReturnsValidationErrorAndDoesNotSave()
    {
        // Arrange — 99 numaralı modül bu pakete ait değil
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ExistingPackage());

        // Act
        var result = await _service.UpdateAsync(ValidUpdateRequest() with { Modules = [Module(99, "Başkası")] });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithItemOfAnotherModule_ReturnsValidationError()
    {
        // Arrange — 201 numaralı kalem 20 numaralı modüle ait, 10'un altında gönderildi
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ExistingPackage());
        var request = ValidUpdateRequest() with
        {
            Modules = [Module(10, "Antrenman") with { Items = [new PackageModuleItemRequest { Id = 201, Text = "Kalem" }] }]
        };

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Fact]
    public async Task UpdateAsync_WithDuplicateHighlightId_ReturnsValidationError()
    {
        // Arrange
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ExistingPackage());
        var request = ValidUpdateRequest() with
        {
            Highlights = [new PackageHighlightRequest { Id = 30, Text = "A" }, new PackageHighlightRequest { Id = 30, Text = "B" }]
        };

        // Act
        var result = await _service.UpdateAsync(request);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Fact]
    public async Task UpdateAsync_WhenAlreadyFeatured_DoesNotReleaseItself()
    {
        // Arrange — öne çıkan zaten bu paket
        var package = ExistingPackage();
        package.IsFeatured = true;
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(package);
        _packages.Setup(r => r.GetFeaturedAsync(PackageLocation.Salonda)).ReturnsAsync(package);

        // Act
        await _service.UpdateAsync(ValidUpdateRequest() with { IsFeatured = true, Modules = [Module(10, "A"), Module(20, "B")] });

        // Assert — ayrı bir "eskisini kaldır" kaydı yapılmadı
        package.IsFeatured.Should().BeTrue();
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    // ---- DeleteAsync / ReorderModulesAsync ----

    [Fact]
    public async Task DeleteAsync_WithExistingId_RemovesPackageWithDetails()
    {
        // Arrange — alt kayıtlar yüklü gelmeli ki cascade ile birlikte silinsinler
        var package = ExistingPackage();
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(package);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _packages.Verify(r => r.Remove(package), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        _packages.Verify(r => r.Remove(It.IsAny<Package>()), Times.Never);
    }

    [Fact]
    public async Task ReorderModulesAsync_WithAllModules_AssignsNewOrder()
    {
        // Arrange
        var package = ExistingPackage();
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(package);

        // Act
        var result = await _service.ReorderModulesAsync(1, [20, 10]);

        // Assert
        result.IsSuccess.Should().BeTrue();
        package.Modules.Single(m => m.Id == 20).SortOrder.Should().Be(1);
        package.Modules.Single(m => m.Id == 10).SortOrder.Should().Be(2);
    }

    [Theory]
    [InlineData(new[] { 10 })]          // eksik modül
    [InlineData(new[] { 10, 10 })]      // tekrar
    [InlineData(new[] { 10, 99 })]      // başka paketin modülü
    [InlineData(new[] { 10, 20, 99 })]  // fazladan
    public async Task ReorderModulesAsync_WithIncompleteOrForeignList_ReturnsValidationError(int[] moduleIds)
    {
        // Arrange
        _packages.Setup(r => r.GetByIdWithDetailsAsync(1)).ReturnsAsync(ExistingPackage());

        // Act
        var result = await _service.ReorderModulesAsync(1, [.. moduleIds]);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    // ---- Test verisi ----

    // 1 numaralı paket: 10 (kalem 101) ve 20 (kalem 201) numaralı modüller, 30 numaralı öne çıkan
    private static Package ExistingPackage() => new()
    {
        Id = 1,
        Name = "Başlangıç",
        Slug = "baslangic",
        Location = PackageLocation.Salonda,
        Modules =
        [
            new PackageModule { Id = 10, Title = "Antrenman", SortOrder = 1, Items = [new PackageModuleItem { Id = 101, Text = "Program" }] },
            new PackageModule { Id = 20, Title = "Beslenme", SortOrder = 2, Items = [new PackageModuleItem { Id = 201, Text = "Plan" }] }
        ],
        Highlights = [new PackageHighlight { Id = 30, Text = "Haftalık kontrol" }]
    };

    private static PackageModuleRequest Module(int? id, string title) =>
        new() { Id = id, Title = title, IconKey = "dumbbell" };

    private static CreatePackageRequest ValidCreateRequest() => new()
    {
        Name = "Başlangıç",
        Slug = "baslangic",
        Subtitle = "6 Aylık Dönüşüm Programı",
        DurationMonths = 6,
        Price = 3450,
        Location = PackageLocation.Salonda,
        ShortDescription = "Kısa açıklama",
        PurchaseUrl = "https://emircoaching.com/satin-al",
        IsActive = true,
        Highlights = [new PackageHighlightRequest { Text = "Haftalık kontrol" }],
        Modules = [Module(null, "Antrenman")]
    };

    private static UpdatePackageRequest ValidUpdateRequest()
    {
        var create = ValidCreateRequest();
        return new UpdatePackageRequest
        {
            Id = 1,
            Name = create.Name,
            Slug = create.Slug,
            Subtitle = create.Subtitle,
            DurationMonths = create.DurationMonths,
            Price = create.Price,
            Location = create.Location,
            ShortDescription = create.ShortDescription,
            PurchaseUrl = create.PurchaseUrl,
            IsActive = create.IsActive,
            Highlights = [new PackageHighlightRequest { Id = 30, Text = "Haftalık kontrol" }],
            Modules = [Module(10, "Antrenman"), Module(20, "Beslenme")]
        };
    }
}
