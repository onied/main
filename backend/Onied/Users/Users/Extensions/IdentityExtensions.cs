using Microsoft.AspNetCore.Identity;
using Users.Data;
using Users.Data.Entities;
using Users.Services.EmailSender;

namespace Users.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityConfigured(
        this IServiceCollection serviceCollection
    )
    {
        serviceCollection
            .AddIdentityApiEndpoints<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        serviceCollection.AddAuthentication(IdentityConstants.BearerScheme);
        serviceCollection.AddAuthorization();
        serviceCollection.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;
        });
        serviceCollection.AddScoped<IEmailSender<AppUser>, LoggingEmailSender>();
        return serviceCollection;
    }
}
