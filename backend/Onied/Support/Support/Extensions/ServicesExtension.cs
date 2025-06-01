using Microsoft.AspNetCore.SignalR;
using Support.Abstractions;
using Support.Events.Abstractions;
using Support.Services;
using Support.Services.GrpcServices;

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
            .AddScoped<IAuthorizationSupportUserService, AuthorizationSupportUserService>()
            .AddScoped<UserChatService>();
        ;
    }

    public static IHttpClientBuilder AddGrpcClients(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        return serviceCollection.AddGrpcClient<UsersGrpc.AuthorizationService.AuthorizationServiceClient>(options =>
            options.Address = new Uri(configuration["UsersGrpcAddress"]!));
    }
}
