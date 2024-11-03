using Support.Profiles;

namespace Support.Extensions;

public static class AutoMapperExtension
{
    public static IServiceCollection AddAutoMapperConfigured(this IServiceCollection services)
    {
        return services.AddAutoMapper(options => options.AddProfile<AppMappingProfile>());
    }
}
