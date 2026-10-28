using AwesomeAssertions;
using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Tests.Common;

public class ChildListSyncTests
{
    // Gerçek entity'lere bağlı kalmamak için yalnızca bu testlerde kullanılan alt kayıt ve istek tipi
    private class Child : BaseEntity
    {
        public string Name { get; set; } = "";
        public int SortOrder { get; set; }
    }

    private record ChildInput(int? Id, string Name);

    // ---- AreKnownIds ----

    // [Theory] aynı testi farklı girdilerle çalıştırır; her [InlineData] ayrı bir test olarak raporlanır
    [Theory]
    [InlineData(new int[0])]          // hiç Id yok (hepsi yeni kayıt)
    [InlineData(new[] { 1 })]         // tek bilinen Id
    [InlineData(new[] { 1, 2, 3 })]   // bilinen Id'lerin hepsi
    public void AreKnownIds_WithKnownUniqueIds_ReturnsTrue(int[] sentIds)
    {
        // Arrange
        var sent = sentIds.Select(id => (int?)id).Append(null);   // sona bir yeni kayıt (null Id) eklenir
        var known = new[] { 1, 2, 3 };

        // Act
        var result = ChildListSync.AreKnownIds(sent, known);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void AreKnownIds_WithForeignId_ReturnsFalse()
    {
        // Arrange — 99 bu üst kayda ait değil (başka paketin modülü gibi)
        int?[] sent = [1, 99];
        int[] known = [1, 2];

        // Act
        var result = ChildListSync.AreKnownIds(sent, known);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AreKnownIds_WithDuplicateId_ReturnsFalse()
    {
        // Arrange
        int?[] sent = [1, 1];
        int[] known = [1, 2];

        // Act
        var result = ChildListSync.AreKnownIds(sent, known);

        // Assert
        result.Should().BeFalse();
    }

    // ---- Sync ----

    [Fact]
    public void Sync_WithMatchingId_UpdatesExistingEntity()
    {
        // Arrange
        var existing = new List<Child> { new() { Id = 1, Name = "Eski" } };
        var inputs = new List<ChildInput> { new(1, "Yeni") };

        // Act
        Sync(existing, inputs, removed: []);

        // Assert — yeni kayıt eklenmedi, var olanın adı değişti
        existing.Should().ContainSingle().Which.Name.Should().Be("Yeni");
    }

    [Fact]
    public void Sync_WithNullId_AddsNewEntity()
    {
        // Arrange
        var existing = new List<Child> { new() { Id = 1, Name = "Var olan" } };
        var inputs = new List<ChildInput> { new(1, "Var olan"), new(null, "Eklenen") };

        // Act
        Sync(existing, inputs, removed: []);

        // Assert — yeni kaydın Id'si henüz 0 (veritabanı kaydedince verir)
        existing.Should().HaveCount(2);
        existing.Should().Contain(c => c.Id == 0 && c.Name == "Eklenen");
    }

    [Fact]
    public void Sync_WithEntityMissingFromInputs_RemovesIt()
    {
        // Arrange
        var existing = new List<Child> { new() { Id = 1 }, new() { Id = 2 } };
        var inputs = new List<ChildInput> { new(1, "Kalan") };
        var removed = new List<Child>();

        // Act
        Sync(existing, inputs, removed);

        // Assert — yalnızca listede olmayan 2 numara silinmek üzere verildi
        removed.Should().ContainSingle().Which.Id.Should().Be(2);
    }

    [Fact]
    public void Sync_AssignsSortOrderFromListPosition()
    {
        // Arrange — istekte sıra ters çevrildi
        var existing = new List<Child> { new() { Id = 1, SortOrder = 1 }, new() { Id = 2, SortOrder = 2 } };
        var inputs = new List<ChildInput> { new(2, "B"), new(1, "A") };

        // Act
        Sync(existing, inputs, removed: []);

        // Assert — sıra 1'den başlar
        existing.Single(c => c.Id == 2).SortOrder.Should().Be(1);
        existing.Single(c => c.Id == 1).SortOrder.Should().Be(2);
    }

    // Servislerin yaptığı gibi Sync'i çağırır: silinenleri listeye toplar, adı ve sırayı yazar
    private static void Sync(List<Child> existing, List<ChildInput> inputs, List<Child> removed) =>
        ChildListSync.Sync(existing, inputs, i => i.Id, removed.Add,
            (child, input, order) =>
            {
                child.Name = input.Name;
                child.SortOrder = order;
            });
}
