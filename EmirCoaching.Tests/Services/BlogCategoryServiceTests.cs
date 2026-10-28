using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

// BlogCategoryService CrudService'ten türer; bu testler tabanın ortak akışını da kapsar
public class BlogCategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IBlogCategoryRepository> _categories = new();
    private readonly Mock<IBlogPostRepository> _posts = new();
    private readonly BlogCategoryService _service;

    // xUnit her test için sınıfı yeniden oluşturur: mock'lar testler arasında paylaşılmaz
    public BlogCategoryServiceTests()
    {
        _uow.Setup(u => u.BlogCategories).Returns(_categories.Object);
        _uow.Setup(u => u.BlogPosts).Returns(_posts.Object);

        _service = new BlogCategoryService(_uow.Object, TestMapper.Create(),
            new CreateBlogCategoryRequestValidator(), new UpdateBlogCategoryRequestValidator(),
            NullLogger<BlogCategoryService>.Instance);
    }

    // ---- GetByIdAsync ----

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnsCategory()
    {
        // Arrange — repository'ye "1 sorulursa bu kategoriyi döndür" diyoruz
        _categories.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new BlogCategory { Id = 1, Name = "Beslenme", Slug = "beslenme" });

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(new BlogCategoryResponse { Id = 1, Name = "Beslenme", Slug = "beslenme" });
    }

    [Fact]
    public async Task GetByIdAsync_WithMissingId_ReturnsNotFound()
    {
        // Arrange — Setup yapılmayan çağrı varsayılanı (null) döner; yine de niyet açık olsun
        _categories.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((BlogCategory?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    // ---- CreateAsync ----

    [Fact]
    public async Task CreateAsync_WithValidRequest_SavesAndReturnsId()
    {
        // Arrange — veritabanı Id vermiş gibi davranmak için AddAsync çağrılınca Id'yi dolduruyoruz
        _categories.Setup(r => r.AddAsync(It.IsAny<BlogCategory>()))
            .Callback<BlogCategory>(c => c.Id = 7)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.CreateAsync(new CreateBlogCategoryRequest { Name = "Beslenme", Slug = "beslenme" });

        // Assert — Verify: mock'un gerçekten (ve kaç kez) çağrıldığını sorar
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(7);
        _categories.Verify(r => r.AddAsync(It.Is<BlogCategory>(c => c.Name == "Beslenme" && c.Slug == "beslenme")), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidRequest_ReturnsValidationErrorAndDoesNotSave()
    {
        // Arrange — ad boş, slug büyük harfli
        var request = new CreateBlogCategoryRequest { Name = "", Slug = "Beslenme" };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        result.Errors.Should().HaveCount(2);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithExistingSlug_ReturnsConflictAndDoesNotSave()
    {
        // Arrange — yeni kayıtta hariç tutulacak Id yok (null)
        _categories.Setup(r => r.SlugExistsAsync("beslenme", null)).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(new CreateBlogCategoryRequest { Name = "Beslenme", Slug = "beslenme" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        _categories.Verify(r => r.AddAsync(It.IsAny<BlogCategory>()), Times.Never);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_OnDatabaseError_ThrowsException()
    {
        // Arrange — servislerde try/catch yok: hata GlobalExceptionHandler'a (500) gitmek üzere yukarı fırlamalı
        _uow.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new InvalidOperationException("Veritabanı hatası"));

        // Act
        var act = () => _service.CreateAsync(new CreateBlogCategoryRequest { Name = "Beslenme", Slug = "beslenme" });

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    // ---- UpdateAsync ----

    [Fact]
    public async Task UpdateAsync_WithValidRequest_UpdatesEntityAndSaves()
    {
        // Arrange
        var category = new BlogCategory { Id = 1, Name = "Eski", Slug = "eski" };
        _categories.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

        // Act
        var result = await _service.UpdateAsync(new UpdateBlogCategoryRequest { Id = 1, Name = "Yeni", Slug = "yeni" });

        // Assert
        result.IsSuccess.Should().BeTrue();
        category.Name.Should().Be("Yeni");
        category.Slug.Should().Be("yeni");
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithExistingSlug_ChecksOtherRecordsOnly()
    {
        // Arrange — güncellemede kaydın kendi slug'ı çakışma sayılmamalı: kontrol kendi Id'sini hariç tutar
        _categories.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new BlogCategory { Id = 1, Slug = "beslenme" });

        // Act
        await _service.UpdateAsync(new UpdateBlogCategoryRequest { Id = 1, Name = "Beslenme", Slug = "beslenme" });

        // Assert
        _categories.Verify(r => r.SlugExistsAsync("beslenme", 1), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateAsync(new UpdateBlogCategoryRequest { Id = 99, Name = "Yeni", Slug = "yeni" });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    // ---- DeleteAsync ----

    [Fact]
    public async Task DeleteAsync_WithEmptyCategory_RemovesAndSaves()
    {
        // Arrange
        var category = new BlogCategory { Id = 1 };
        _categories.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);
        _posts.Setup(r => r.AnyInCategoryAsync(1)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _categories.Verify(r => r.Remove(category), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithPostsInCategory_ReturnsConflictAndDoesNotRemove()
    {
        // Arrange
        _categories.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new BlogCategory { Id = 1 });
        _posts.Setup(r => r.AnyInCategoryAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
        _categories.Verify(r => r.Remove(It.IsAny<BlogCategory>()), Times.Never);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.DeleteAsync(99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        _categories.Verify(r => r.Remove(It.IsAny<BlogCategory>()), Times.Never);
    }
}
