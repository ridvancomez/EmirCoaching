using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Services.Concrete;
using EmirCoaching.DataAccess.Repositories.Abstract;
using EmirCoaching.DataAccess.Repositories.UnitOfWork;
using EmirCoaching.Entities.Identity;
using EmirCoaching.Tests.Helpers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace EmirCoaching.Tests.Services;

public class CoachProfileServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IAppUserRepository> _users = new();
    private readonly CoachProfileService _service;

    public CoachProfileServiceTests()
    {
        _uow.Setup(u => u.Users).Returns(_users.Object);
        _service = new CoachProfileService(_uow.Object, TestMapper.Create(), NullLogger<CoachProfileService>.Instance);
    }

    [Fact]
    public async Task GetAsync_WithCoach_ReturnsCoachSectionFields()
    {
        // Arrange
        _users.Setup(r => r.GetCoachAsync()).ReturnsAsync(new AppUser
        {
            FullName = "İbrahim Emir", Title = "Kurucu & Baş Antrenör", IsCoach = true,
            CoachTitle = "Online Fitness & Beslenme Koçu", CoachExperience = "8+ yıl",
            CoachStory = "Birinci.\n\nİkinci.", CoachVideoCoverPath = "/uploads/koc/kapak.jpg"
        });

        // Act
        var result = await _service.GetAsync();

        // Assert — blog yazar unvanı değil, koç unvanı
        result.IsSuccess.Should().BeTrue();
        result.Value!.Title.Should().Be("Online Fitness & Beslenme Koçu");
        result.Value.StoryParagraphs.Should().Equal("Birinci.", "İkinci.");
        result.Value.VideoCoverUrl.Should().Be($"{TestMapper.PublicBaseUrl}/uploads/koc/kapak.jpg");
    }

    [Fact]
    public async Task GetAsync_WithoutCoach_ReturnsNotFound()
    {
        // Act
        var result = await _service.GetAsync();

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }
}
