using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Services;

namespace Support.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddSingleton<IUserIdProvider, QueryUserIdProvider>()
            .AddScoped<IMessageGenerator, MessageGenerator>()
            .AddScoped<IChatHubClientSender, ChatHubClientSenderService>()
            .AddScoped<IChatService, ChatService>()
            .AddScoped<ISupportService, SupportService>()
            .AddScoped<IAuthorizationSupportUserService, AuthorizationSupportUserService>();
    }
}
