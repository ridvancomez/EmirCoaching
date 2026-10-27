using EmirCoaching.Business.Common;
using EmirCoaching.Entities.Common;

namespace EmirCoaching.Business.Dtos;

public record PackageHighlightResponse
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public bool IsLocked { get; init; }
    public int SortOrder { get; init; }
}

public record PackageResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Subtitle { get; init; } = string.Empty;
    public int DurationMonths { get; init; }
    public decimal Price { get; init; }
    public PackageLocation Location { get; init; }
    public string ShortDescription { get; init; } = string.Empty;
    public string PurchaseUrl { get; init; } = string.Empty;
    public bool IsFeatured { get; init; }
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public IReadOnlyList<PackageHighlightResponse> Highlights { get; init; } = [];
}

public record PackageModuleItemResponse
{
    public int Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public bool IsLocked { get; init; }
    public int SortOrder { get; init; }
}

public record PackageModuleResponse
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string IconKey { get; init; } = string.Empty;
    public int SortOrder { get; init; }
    public IReadOnlyList<PackageModuleItemResponse> Items { get; init; } = [];
}

public record PackageDetailResponse : PackageResponse
{
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public IReadOnlyList<PackageModuleResponse> Modules { get; init; } = [];
    public IReadOnlyList<TestimonialResponse> Testimonials { get; init; } = [];
    public int TotalItemCount { get; init; } // "Toplam 15 kalem" (kilitli kalemler hariç)
}

// --- Admin girdileri ---
// Alt kayıtlarda Id: güncellemede var olan kaydı numarası değişmeden günceller; boşsa yeni kayıt eklenir.
// Listede olmayan eski kayıt silinir. Oluşturmada Id yok sayılır.

public record PackageModuleItemRequest
{
    public int? Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public bool IsLocked { get; init; }
}

public record PackageHighlightRequest
{
    public int? Id { get; init; }
    public string Text { get; init; } = string.Empty;
    public bool IsLocked { get; init; }
}

public record PackageModuleRequest
{
    public int? Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string IconKey { get; init; } = string.Empty;
    public List<PackageModuleItemRequest> Items { get; init; } = [];
}

// Liste sırası SortOrder olarak kaydedilir
public record CreatePackageRequest
{
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Subtitle { get; init; } = string.Empty;
    public int DurationMonths { get; init; }
    public decimal Price { get; init; }
    public PackageLocation Location { get; init; }
    public string ShortDescription { get; init; } = string.Empty;
    public string PurchaseUrl { get; init; } = string.Empty;
    public bool IsFeatured { get; init; } // işaretlenirse aynı kategorideki önceki öne çıkan kaldırılır
    public int SortOrder { get; init; }
    public bool IsActive { get; init; }
    public string? MetaTitle { get; init; }
    public string? MetaDescription { get; init; }
    public List<PackageHighlightRequest> Highlights { get; init; } = [];
    public List<PackageModuleRequest> Modules { get; init; } = [];
}

// Modüller ve öne çıkanlar gönderilen listeyle eşitlenir (Id'li olan güncellenir, Id'siz eklenir, eksik olan silinir)
public record UpdatePackageRequest : CreatePackageRequest, IUpdateRequest
{
    public int Id { get; init; }
}
