using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record PackageHighlightDto
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public int SortOrder { get; init; }
}

public record PackageDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Subtitle { get; init; } = string.Empty;
    public int DurationMonths { get; init; }
    public decimal Price { get; init; }
    public PackageLocation Location { get; init; }
    public string ShortDescription { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public IReadOnlyList<PackageHighlightDto> Highlights { get; init; } = [];
}

public record PackageModuleItemDto
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public ModuleItemState State { get; init; }
    public string? LockedLabel { get; init; }
    public int SortOrder { get; init; }
}

public record PackageModuleDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string IconKey { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public IReadOnlyList<PackageModuleItemDto> Items { get; init; } = [];
}

public record TestimonialDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
}

public record PackageDetailDto : PackageDto
{
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public IReadOnlyList<PackageModuleDto> Modules { get; init; } = [];
    public IReadOnlyList<TestimonialDto> Testimonials { get; init; } = [];
    public int TotalItemCount { get; init; } // "Toplam 15 kalem"
}

// --- Admin girdileri ---

public record PackageModuleItemInputDto
{
    public string Text { get; init; } = string.Empty;
    public ModuleItemState State { get; init; }
    public string? LockedLabel { get; init; }
}

public record PackageModuleInputDto
{
    public string Title { get; init; } = string.Empty;
    public string IconKey { get; init; } = string.Empty;
    public List<PackageModuleItemInputDto> Items { get; init; } = [];
}

// Liste sırası SortOrder olarak kaydedilir
public record CreatePackageDto
{
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Subtitle { get; init; } = string.Empty;
    public int DurationMonths { get; init; }
    public decimal Price { get; init; }
    public PackageLocation Location { get; init; }
    public string ShortDescription { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public List<string> Highlights { get; init; } = [];
    public List<PackageModuleInputDto> Modules { get; init; } = [];
}

// Modüller ve öne çıkanlar gönderilen listeyle değiştirilir
public record UpdatePackageDto : CreatePackageDto
{
    public int Id { get; init; }
}
