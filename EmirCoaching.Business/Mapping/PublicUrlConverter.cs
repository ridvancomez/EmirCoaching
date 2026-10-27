using AutoMapper;
using EmirCoaching.Business.Common;
using Microsoft.Extensions.Options;

namespace EmirCoaching.Business.Mapping;

// DB'deki göreli dosya yolunu yanıtta tam URL'ye çevirir (kural: StorageOptions.ToPublicUrl).
// İki sınıf yalnızca nullability için: isteğe bağlı alanlar null kalır, zorunlu alanlar string döner.

// İsteğe bağlı görseller (CoverImageUrl, AvatarUrl, ...)
public class PublicUrlConverter(IOptions<StorageOptions> options) : IValueConverter<string?, string?>
{
    public string? Convert(string? sourceMember, ResolutionContext context) =>
        sourceMember is null ? null : options.Value.ToPublicUrl(sourceMember);
}

// Zorunlu görseller (BeforeImageUrl, ThumbnailUrl, ...)
public class RequiredPublicUrlConverter(IOptions<StorageOptions> options) : IValueConverter<string, string>
{
    public string Convert(string sourceMember, ResolutionContext context) => options.Value.ToPublicUrl(sourceMember);
}
