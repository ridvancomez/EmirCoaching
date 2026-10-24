using AutoMapper;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Entities.Identity;

namespace EmirCoaching.Business.Mapping;

// Girdi DTO'larından entity'ye yalnızca düz alanlar eşlenir; alt koleksiyonlar servislerde kurulur.
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Package
        CreateMap<PackageHighlight, PackageHighlightDto>();
        CreateMap<PackageModuleItem, PackageModuleItemDto>();
        CreateMap<PackageModule, PackageModuleDto>();
        CreateMap<Testimonial, TestimonialDto>();
        CreateMap<Package, PackageDto>();
        CreateMap<Package, PackageDetailDto>()
            .ForMember(d => d.TotalItemCount, o => o.MapFrom(s => s.Modules.Sum(m => m.Items.Count)));

        CreateMap<CreatePackageDto, Package>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Modules, o => o.Ignore())
            .ForMember(d => d.Highlights, o => o.Ignore())
            .ForMember(d => d.Testimonials, o => o.Ignore())
            .Include<UpdatePackageDto, Package>();
        CreateMap<UpdatePackageDto, Package>();

        // Blog
        CreateMap<AppUser, BlogAuthorDto>();
        CreateMap<BlogPost, BlogPostDto>();
        CreateMap<BlogPost, BlogPostDetailDto>()
            .ForMember(d => d.RelatedPosts, o => o.Ignore());

        CreateMap<CreateBlogPostDto, BlogPost>()
            .ForMember(d => d.Id, o => o.Ignore())
            .Include<UpdateBlogPostDto, BlogPost>();
        CreateMap<UpdateBlogPostDto, BlogPost>();

        // Transformation
        CreateMap<TransformationMetric, TransformationMetricDto>();
        CreateMap<Transformation, TransformationDto>();
        CreateMap<Transformation, TransformationDetailDto>();

        CreateMap<CreateTransformationDto, Transformation>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Metrics, o => o.Ignore())
            .Include<UpdateTransformationDto, Transformation>();
        CreateMap<UpdateTransformationDto, Transformation>();

        // Faq / SiteStat / SiteSetting
        CreateMap<Faq, FaqDto>();
        CreateMap<CreateFaqDto, Faq>()
            .ForMember(d => d.Id, o => o.Ignore())
            .Include<UpdateFaqDto, Faq>();
        CreateMap<UpdateFaqDto, Faq>();

        CreateMap<SiteStat, SiteStatDto>();
        CreateMap<CreateSiteStatDto, SiteStat>()
            .ForMember(d => d.Id, o => o.Ignore())
            .Include<UpdateSiteStatDto, SiteStat>();
        CreateMap<UpdateSiteStatDto, SiteStat>();

        CreateMap<SiteSetting, SiteSettingDto>();
        CreateMap<UpdateSiteSettingDto, SiteSetting>();

        // Yasal metinler ve formlar
        CreateMap<LegalDocument, LegalDocumentDto>();

        CreateMap<CreateContactMessageDto, ContactMessage>();
        CreateMap<ContactMessage, ContactMessageDto>();

        CreateMap<CreateDietFormDto, DietFormSubmission>();
        CreateMap<DietFormSubmission, DietFormDto>();
    }
}
