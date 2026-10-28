using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Business.Services.Abstract;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.Business.Validation;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Entities.Identity;
using EmirCoaching.Tests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class BlogPostServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IBlogPostRepository> _posts = new();
    private readonly Mock<IBlogCategoryRepository> _categories = new();
    private readonly Mock<IPackageRepository> _packages = new();
    private readonly Mock<UserManager<AppUser>> _userManager = TestUserManager.Create();
    private readonly Mock<IUploadCleanupService> _uploadCleanup = new();
    private readonly BlogPostService _service;

    public BlogPostServiceTests()
    {
        _uow.Setup(u => u.BlogPosts).Returns(_posts.Object);
        _uow.Setup(u => u.BlogCategories).Returns(_categories.Object);
        _uow.Setup(u => u.Packages).Returns(_packages.Object);

        // Varsayılan: 1 numaralı kategori var; etiket sorgusu boş döner
        _categories.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new BlogCategory { Id = 1 });
        _posts.Setup(r => r.GetTagsBySlugsAsync(It.IsAny<IReadOnlyCollection<string>>())).ReturnsAsync([]);

        _service = new BlogPostService(_uow.Object, TestMapper.Create(), _userManager.Object,
            new CreateBlogPostRequestValidator(), new UpdateBlogPostRequestValidator(),
            _uploadCleanup.Object, NullLogger<BlogPostService>.Instance);
    }

    // ---- CreateAsync ----

    [Fact]
    public async Task CreateAsync_WithValidRequest_CreatesDraft()
    {
        // Arrange
        BlogPost? added = null;
        _posts.Setup(r => r.AddAsync(It.IsAny<BlogPost>())).Callback<BlogPost>(p => added = p);

        // Act
        var result = await _service.CreateAsync(ValidCreateRequest());

        // Assert — yeni yazı taslaktır, yayına PublishAsync ile alınır
        result.IsSuccess.Should().BeTrue();
        added!.IsPublished.Should().BeFalse();
        added.PublishedAt.Should().BeNull();
        _uow.Verify(u => u.CommitAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithMissingCategory_ReturnsValidationError()
    {
        // Act — 5 numaralı kategori yok
        var result = await _service.CreateAsync(ValidCreateRequest() with { CategoryId = 5 });

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Validation);
        result.Errors.Should().Contain("Kategori bulunamadı.");
    }

    [Fact]
    public async Task CreateAsync_WithMissingAuthorAndPackage_ReturnsBothErrors()
    {
        // Act — kullanıcı ve paket mock'ta tanımlı değil, null döner
        var result = await _service.CreateAsync(ValidCreateRequest() with { AuthorId = 7, RecommendedPackageId = 9 });

        // Assert
        result.Errors.Should().BeEquivalentTo(["Yazar bulunamadı.", "Önerilen paket bulunamadı."]);
        _posts.Verify(r => r.AddAsync(It.IsAny<BlogPost>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithExistingSlug_ReturnsConflict()
    {
        // Arrange
        _posts.Setup(r => r.SlugExistsAsync("makro-besinler", null)).ReturnsAsync(true);

        // Act
        var result = await _service.CreateAsync(ValidCreateRequest());

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.Conflict);
    }

    [Fact]
    public async Task CreateAsync_WithTags_ReusesExistingAndCreatesMissing()
    {
        // Arrange — "beslenme" etiketi zaten var; "#Beslenme" ve "beslenme" aynı etikettir
        var existingTag = new BlogTag { Id = 3, Name = "Beslenme", Slug = "beslenme" };
        _posts.Setup(r => r.GetTagsBySlugsAsync(It.IsAny<IReadOnlyCollection<string>>())).ReturnsAsync([existingTag]);
        BlogPost? added = null;
        _posts.Setup(r => r.AddAsync(It.IsAny<BlogPost>())).Callback<BlogPost>(p => added = p);

        // Act
        await _service.CreateAsync(ValidCreateRequest() with { Tags = ["#Beslenme", "beslenme", "Kas Gelişimi"] });

        // Assert
        added!.Tags.Should().HaveCount(2);
        added.Tags.Should().Contain(t => t.BlogTag == existingTag);
        added.Tags.Should().Contain(t => t.BlogTag.Id == 0 && t.BlogTag.Name == "Kas Gelişimi" && t.BlogTag.Slug == "kas-gelisimi");
    }

    [Fact]
    public async Task CreateAsync_WhenFeatured_ReleasesPreviousFeaturedInSameCategory()
    {
        // Arrange
        var previous = new BlogPost { Id = 2, IsFeatured = true, CategoryId = 1 };
        _posts.Setup(r => r.GetFeaturedAsync(1)).ReturnsAsync(previous);

        // Act
        await _service.CreateAsync(ValidCreateRequest() with { IsFeatured = true });

        // Assert
        previous.IsFeatured.Should().BeFalse();
        _uow.Verify(u => u.SaveChangesAsync(), Times.Exactly(2));
    }

    // ---- UpdateAsync ----

    [Fact]
    public async Task UpdateAsync_WhenCoverImageChanges_DeletesOldImageAfterSave()
    {
        // Arrange
        var post = ExistingPost();
        _posts.Setup(r => r.GetByIdWithTagsAsync(1)).ReturnsAsync(post);

        // Act
        var result = await _service.UpdateAsync(ValidUpdateRequest() with { CoverImagePath = "/uploads/blog/yeni.jpg" });

        // Assert
        result.IsSuccess.Should().BeTrue();
        post.CoverImagePath.Should().Be("/uploads/blog/yeni.jpg");
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p => p.SequenceEqual(new[] { "/uploads/blog/eski.jpg" }))), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenSaveFails_RollsBackAndKeepsOldImage()
    {
        // Arrange
        _posts.Setup(r => r.GetByIdWithTagsAsync(1)).ReturnsAsync(ExistingPost());
        _uow.Setup(u => u.SaveChangesAsync()).ThrowsAsync(new InvalidOperationException());

        // Act
        var act = () => _service.UpdateAsync(ValidUpdateRequest() with { CoverImagePath = "/uploads/blog/yeni.jpg" });

        // Assert — kayıt başarısızsa eski görsel hâlâ kullanımdadır, silinmemeli
        await act.Should().ThrowAsync<InvalidOperationException>();
        _uow.Verify(u => u.RollbackAsync(), Times.Once);
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.IsAny<IEnumerable<string>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WithTagRemovedFromList_RemovesTagLink()
    {
        // Arrange — yazıda "beslenme" ve "uyku" var, istekte yalnızca "beslenme"
        var post = ExistingPost();
        _posts.Setup(r => r.GetByIdWithTagsAsync(1)).ReturnsAsync(post);

        // Act
        await _service.UpdateAsync(ValidUpdateRequest() with { Tags = ["Beslenme"] });

        // Assert
        post.Tags.Should().ContainSingle().Which.BlogTag.Slug.Should().Be("beslenme");
    }

    [Fact]
    public async Task UpdateAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.UpdateAsync(ValidUpdateRequest());

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    // ---- DeleteAsync ----

    [Fact]
    public async Task DeleteAsync_WithExistingId_RemovesPostAndReleasesCoverImage()
    {
        // Arrange
        var post = ExistingPost();
        _posts.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(post);

        // Act
        var result = await _service.DeleteAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _posts.Verify(r => r.Remove(post), Times.Once);
        _uploadCleanup.Verify(c => c.DeleteUnusedAsync(It.Is<IEnumerable<string>>(p => p.Contains("/uploads/blog/eski.jpg"))), Times.Once);
    }

    // ---- PublishAsync / UnpublishAsync ----

    [Fact]
    public async Task PublishAsync_FirstTime_SetsPublishedAt()
    {
        // Arrange
        var post = new BlogPost { Id = 1 };
        _posts.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(post);

        // Act
        await _service.PublishAsync(1);

        // Assert
        post.IsPublished.Should().BeTrue();
        post.PublishedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task PublishAsync_AfterUnpublish_KeepsFirstPublishDate()
    {
        // Arrange
        var firstPublished = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        var post = new BlogPost { Id = 1, IsPublished = false, PublishedAt = firstPublished };
        _posts.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(post);

        // Act
        await _service.PublishAsync(1);

        // Assert
        post.PublishedAt.Should().Be(firstPublished);
    }

    [Fact]
    public async Task UnpublishAsync_ReturnsToDraftAndKeepsPublishDate()
    {
        // Arrange
        var firstPublished = new DateTime(2026, 1, 15, 0, 0, 0, DateTimeKind.Utc);
        var post = new BlogPost { Id = 1, IsPublished = true, PublishedAt = firstPublished };
        _posts.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(post);

        // Act
        var result = await _service.UnpublishAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        post.IsPublished.Should().BeFalse();
        post.PublishedAt.Should().Be(firstPublished);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PublishAsync_WithMissingId_ReturnsNotFound()
    {
        // Act
        var result = await _service.PublishAsync(99);

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
        _uow.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    // ---- Test verisi ----

    private static BlogPost ExistingPost()
    {
        var post = new BlogPost { Id = 1, CategoryId = 1, Slug = "makro-besinler", CoverImagePath = "/uploads/blog/eski.jpg" };
        post.Tags.Add(new BlogPostTag { BlogPost = post, BlogTag = new BlogTag { Id = 3, Name = "Beslenme", Slug = "beslenme" } });
        post.Tags.Add(new BlogPostTag { BlogPost = post, BlogTag = new BlogTag { Id = 4, Name = "Uyku", Slug = "uyku" } });
        return post;
    }

    private static CreateBlogPostRequest ValidCreateRequest() => new()
    {
        Title = "Makro Besinler",
        Slug = "makro-besinler",
        Summary = "Özet",
        ContentHtml = "<p>İçerik</p>",
        ReadingMinutes = 5,
        CategoryId = 1
    };

    private static UpdateBlogPostRequest ValidUpdateRequest() => new()
    {
        Id = 1,
        Title = "Makro Besinler",
        Slug = "makro-besinler",
        Summary = "Özet",
        ContentHtml = "<p>İçerik</p>",
        CoverImagePath = "/uploads/blog/eski.jpg",
        ReadingMinutes = 5,
        CategoryId = 1,
        Tags = ["Beslenme", "Uyku"]
    };
}
