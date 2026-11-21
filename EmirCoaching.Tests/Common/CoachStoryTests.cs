using AwesomeAssertions;
using EmirCoaching.Business.Common;

namespace EmirCoaching.Tests.Common;

public class CoachStoryTests
{
    [Fact]
    public void Paragraphs_SplitsOnBlankLinesAndTrims()
    {
        // Act — panel textarea'sı Windows satır sonu da gönderebilir
        var paragraphs = CoachStory.Paragraphs("  Birinci paragraf.\r\n\r\nİkinci\nsatır devam.\n\n\n\nÜçüncü  ");

        // Assert — tek satır sonu paragrafı bölmez
        paragraphs.Should().Equal("Birinci paragraf.", "İkinci\nsatır devam.", "Üçüncü");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Paragraphs_WithEmptyStory_ReturnsEmpty(string? story)
    {
        CoachStory.Paragraphs(story).Should().BeEmpty();
    }
}
