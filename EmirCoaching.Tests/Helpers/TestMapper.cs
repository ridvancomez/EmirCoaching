using AutoMapper;
using EmirCoaching.Business.Common;
using EmirCoaching.Business.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace EmirCoaching.Tests.Helpers;

// Gerçek MappingProfile'ı API'deki gibi DI ile kurar (PublicUrlConverter StorageOptions'ı DI'dan alır)
public static class TestMapper
{
    public const string PublicBaseUrl = "https://api.test";

    public static IMapper Create()
    {
        var services = new ServiceCollection();
        services.Configure<StorageOptions>(o => o.PublicBaseUrl = PublicBaseUrl);
        services.AddAutoMapper(typeof(MappingProfile));
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }
}
