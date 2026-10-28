using AwesomeAssertions;
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

public class FaqServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IFaqRepository> _faqs = new();
    private readonly FaqService _service;

    public FaqServiceTests()
    {
        _uow.Setup(u => u.Faqs).Returns(_faqs.Object);
        _service = new FaqService(_uow.Object, TestMapper.Create(),
            new CreateFaqRequestValidator(), new UpdateFaqRequestValidator(), NullLogger<FaqService>.Instance);
    }

    [Fact]
    public async Task GetAllAsync_GroupsByGroupKeepingSortOrderInside()
    {
        // Arrange — repository SortOrder'a göre sıralı döner; servis gruba göre toplar (sıralama kararlı)
        _faqs.Setup(r => r.GetOrderedAsync()).ReturnsAsync(
        [
            new Faq { Id = 1, Group = FaqGroup.Diyet, SortOrder = 1 },
            new Faq { Id = 2, Group = FaqGroup.Genel, SortOrder = 2 },
            new Faq { Id = 3, Group = FaqGroup.Diyet, SortOrder = 3 },
            new Faq { Id = 4, Group = FaqGroup.Genel, SortOrder = 4 }
        ]);

        // Act
        var result = await _service.GetAllAsync();

        // Assert — Genel (0) önce, Diyet (2) sonra; grup içinde sıra korunur
        result.Value!.Select(f => f.Id).Should().Equal(2, 4, 1, 3);
    }
}
