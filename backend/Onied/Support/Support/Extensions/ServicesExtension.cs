using Support.Abstractions;
using Support.Services;

namespace Support.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<ISupportService, SupportService>();
        services.AddScoped<IAuthorizationSupportUserService, AuthorizationSupportUserService>();
        return services;
    }
}
