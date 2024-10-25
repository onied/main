
using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Services;

namespace Support.Extensions;

public static class ServicesExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services.AddSingleton<IUserIdProvider, QueryUserIdProvider>()
            .AddScoped<ISystemMessageGenerator, SystemMessageGenerator>()
            .AddScoped<IChatManagementService, ChatManagementService>();
    }
}
