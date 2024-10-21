using Microsoft.Extensions.DependencyInjection;
using Support.Data.Abstractions;
using Support.Data.Repositories;

namespace Support.Data;

public static class BuilderServicesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IChatRepository, ChatRepository>()
            .AddScoped<IMessageRepository, MessageRepository>()
            .AddScoped<ISupportUserRepository, SupportUserRepository>();
    }
}
