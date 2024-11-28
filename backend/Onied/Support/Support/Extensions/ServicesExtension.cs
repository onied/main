using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Events.Abstractions;
using Support.Services;

namespace Support.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<IChatHubClientSender, ChatHubClientSenderService>()
            .AddSingleton<IUserIdProvider, QueryUserIdProvider>()
            .AddScoped<IChatService, ChatService>()
            .AddScoped<ISupportService, SupportService>()
            .AddScoped<IAuthorizationSupportUserService, AuthorizationSupportUserService>();
    }
}
