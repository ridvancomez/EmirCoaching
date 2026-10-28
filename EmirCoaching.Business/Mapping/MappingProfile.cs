using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Dtos;
using EmirCoaching.Entities.Common;
using EmirCoaching.Entities.Entities;
using EmirCoaching.Entities.Identity;

namespace EmirCoaching.Business.Mapping;

// Request'lerden entity'ye yalnızca düz alanlar eşlenir; alt koleksiyonlar servislerde kurulur.
// Response'larda dosya yolları (*Path) PublicUrlConverter ile *Url alanına tam URL olarak çıkar.
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Package
        CreateMap<PackageHighlight, PackageHighlightResponse>();
        CreateMap<PackageModuleItem, PackageModuleItemResponse>();
        CreateMap<PackageModule, PackageModuleResponse>();
        CreateMap<Package, PackageResponse>();
        CreateMap<Package, PackageDetailResponse>()
            .ForMember(d => d.TotalItemCount, o => o.MapFrom(s => s.Modules.Sum(m => m.Items.Count(i => !i.IsLocked))));
        MapRequests<CreatePackageRequest, UpdatePackageRequest, Package>()
            .ForMember(d => d.Modules, o => o.Ignore())
            .ForMember(d => d.Highlights, o => o.Ignore())
            .ForMember(d => d.Testimonials, o => o.Ignore());

        CreateMap<Testimonial, TestimonialResponse>();
        MapRequests<CreateTestimonialRequest, UpdateTestimonialRequest, Testimonial>()
            .ForMember(d => d.Package, o => o.Ignore());

        // Kimlik doğrulama (roller UserManager'dan ayrıca eklenir)
        CreateMap<AppUser, CurrentUserResponse>()
            .ForMember(d => d.Roles, o => o.Ignore())
            .ForMember(d => d.AvatarUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.AvatarPath));
        CreateMap<UpdateProfileRequest, AppUser>(); // yalnızca aynı adlı alanlar: ad, unvan, biyografi, fotoğraf

        // Blog
        CreateMap<AppUser, BlogAuthorResponse>()
            .ForMember(d => d.AvatarUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.AvatarPath));
        CreateMap<BlogPost, BlogPostResponse>()
            .ForMember(d => d.CoverImageUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.CoverImagePath));
        CreateMap<BlogPost, BlogPostDetailResponse>()
            .IncludeBase<BlogPost, BlogPostResponse>()
            .ForMember(d => d.Tags, o => o.MapFrom(s => s.Tags.Select(t => t.BlogTag.Name)))
            .ForMember(d => d.RelatedPosts, o => o.Ignore());
        MapRequests<CreateBlogPostRequest, UpdateBlogPostRequest, BlogPost>()
            .ForMember(d => d.Tags, o => o.Ignore());

        CreateMap<BlogCategory, BlogCategoryResponse>();
        MapRequests<CreateBlogCategoryRequest, UpdateBlogCategoryRequest, BlogCategory>()
            .ForMember(d => d.BlogPosts, o => o.Ignore());

        // Transformation
        CreateMap<TransformationMetric, TransformationMetricResponse>();
        CreateMap<Transformation, TransformationResponse>()
            .ForMember(d => d.BeforeImageUrl, o => o.ConvertUsing<RequiredPublicUrlConverter, string>(s => s.BeforeImagePath))
            .ForMember(d => d.AfterImageUrl, o => o.ConvertUsing<RequiredPublicUrlConverter, string>(s => s.AfterImagePath))
            .ForMember(d => d.BeforeSquareUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.BeforeSquarePath))
            .ForMember(d => d.AfterSquareUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.AfterSquarePath));
        CreateMap<Transformation, TransformationDetailResponse>()
            .IncludeBase<Transformation, TransformationResponse>();
        MapRequests<CreateTransformationRequest, UpdateTransformationRequest, Transformation>()
            .ForMember(d => d.Metrics, o => o.Ignore());

        CreateMap<TransformationType, TransformationTypeResponse>();
        MapRequests<CreateTransformationTypeRequest, UpdateTransformationTypeRequest, TransformationType>()
            .ForMember(d => d.Transformations, o => o.Ignore());

        // Site içeriği
        CreateMap<Faq, FaqResponse>();
        MapRequests<CreateFaqRequest, UpdateFaqRequest, Faq>();

        CreateMap<SiteStat, SiteStatResponse>();
        MapRequests<CreateSiteStatRequest, UpdateSiteStatRequest, SiteStat>();

        CreateMap<SiteSetting, SiteSettingResponse>();
        CreateMap<UpdateSiteSettingRequest, SiteSetting>();

        CreateMap<VideoQuestion, VideoQuestionResponse>()
            .ForMember(d => d.ThumbnailUrl, o => o.ConvertUsing<RequiredPublicUrlConverter, string>(s => s.ThumbnailPath));
        MapRequests<CreateVideoQuestionRequest, UpdateVideoQuestionRequest, VideoQuestion>();

        CreateMap<SocialLink, SocialLinkResponse>();
        MapRequests<CreateSocialLinkRequest, UpdateSocialLinkRequest, SocialLink>();

        CreateMap<SocialPost, SocialPostResponse>()
            .ForMember(d => d.ImageUrl, o => o.ConvertUsing<RequiredPublicUrlConverter, string>(s => s.ImagePath));
        MapRequests<CreateSocialPostRequest, UpdateSocialPostRequest, SocialPost>()
            .ForMember(d => d.ExternalId, o => o.Ignore())
            .ForMember(d => d.LikeCount, o => o.Ignore());

        CreateMap<PageContent, PageContentResponse>()
            .ForMember(d => d.BackgroundImageUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.BackgroundImagePath));
        CreateMap<UpdatePageContentRequest, PageContent>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.PageKey, o => o.Ignore());

        CreateMap<DietProgram, DietProgramResponse>()
            .ForMember(d => d.DietitianPhotoUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.DietitianPhotoPath));
        CreateMap<UpdateDietProgramRequest, DietProgram>()
            .ForMember(d => d.Id, o => o.Ignore());

        CreateMap<DietProgramStep, DietProgramStepResponse>()
            .ForMember(d => d.IconName, o => o.MapFrom(s => DietProgramStepIcons.NameOf(s.Icon)));
        MapRequests<CreateDietProgramStepRequest, UpdateDietProgramStepRequest, DietProgramStep>();

        // Yasal metinler ve formlar
        CreateMap<LegalDocument, LegalDocumentResponse>()
            .ForMember(d => d.FileUrl, o => o.ConvertUsing<PublicUrlConverter, string?>(s => s.FilePath));
        CreateMap<LegalDocumentRevision, LegalDocumentRevisionResponse>();

        CreateMap<CreateContactMessageRequest, ContactMessage>()
            .ForMember(d => d.Location, o => o.MapFrom(s => ParseLocation(s.Location)));
        CreateMap<ContactMessage, ContactMessageResponse>();

        CreateMap<CreateDietFormSubmissionRequest, DietFormSubmission>();
        CreateMap<DietFormSubmission, DietFormSubmissionResponse>();

        CreateMap<NewsletterSubscriber, NewsletterSubscriberResponse>();

        CreateMap<LoginAttempt, LoginAttemptResponse>(); // UserEmail/UserFullName → User.Email/FullName
    }

    // Create{Entity}Request → entity (Id yok sayılır); Update{Entity}Request aynı kuralları miras alır.
    // Dönen ifadeye eklenen Ignore'lar güncelleme eşlemesine de uygulanır.
    private IMappingExpression<TCreateRequest, TEntity> MapRequests<TCreateRequest, TUpdateRequest, TEntity>()
        where TUpdateRequest : TCreateRequest
        where TEntity : BaseEntity
    {
        CreateMap<TUpdateRequest, TEntity>();
        return CreateMap<TCreateRequest, TEntity>()
            .ForMember(d => d.Id, o => o.Ignore())
            .Include<TUpdateRequest, TEntity>();
    }

    // Front-end "evde" / "salonda" değerleri (validator geçersiz değeri önceden reddeder)
    private static PackageLocation? ParseLocation(string? value) => value switch
    {
        "evde" => PackageLocation.Evde,
        "salonda" => PackageLocation.Salonda,
        _ => null
    };
}
