using EmirCoaching.Entities.Common;

namespace EmirCoaching.Entities.Entities;

public class Package : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public decimal Price { get; set; }
    public PackageLocation Location { get; set; }
    public string ShortDescription { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public ICollection<PackageModule> Modules { get; set; } = new List<PackageModule>();
    public ICollection<PackageHighlight> Highlights { get; set; } = new List<PackageHighlight>();
    public ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
