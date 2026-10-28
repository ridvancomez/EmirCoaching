using AwesomeAssertions;
using EmirCoaching.Business.Common;

namespace EmirCoaching.Tests.Common;

public class BulkDeleteTests
{
    [Fact]
    public void Validate_WithEmptyList_ReturnsValidationError()
    {
        BulkDelete.Validate([]).Errors.Should().Equal("Silinecek kayıt seçilmedi.");
    }

    [Fact]
    public void Validate_WithMoreThanMax_ReturnsValidationError()
    {
        BulkDelete.Validate(Enumerable.Range(1, BulkDelete.MaxCount + 1).ToList()).ErrorType.Should().Be(ResultErrorType.Validation);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void Validate_WithNonPositiveId_ReturnsValidationError(int id)
    {
        BulkDelete.Validate([1, id]).Errors.Should().Equal("Geçersiz kayıt numarası.");
    }

    [Fact]
    public async Task LoadAsync_WithMissingRecord_ReturnsNotFound()
    {
        // Act — 2 numara yok
        var result = await BulkDelete.LoadAsync([1, 2], id => Task.FromResult(id == 1 ? "bir" : null), "Bulunamadı.");

        // Assert
        result.ErrorType.Should().Be(ResultErrorType.NotFound);
    }

    [Fact]
    public async Task LoadAsync_WithDuplicateIds_LoadsEachOnce()
    {
        // Arrange
        var loads = new List<int>();

        // Act
        var result = await BulkDelete.LoadAsync([1, 1, 2], id => { loads.Add(id); return Task.FromResult<string?>(id.ToString()); }, "Bulunamadı.");

        // Assert
        result.Value.Should().Equal("1", "2");
        loads.Should().Equal(1, 2);
    }
}
