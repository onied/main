using Microsoft.AspNetCore.Identity;
using Users.Data;

namespace Users.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityConfigured(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddIdentityApiEndpoints<AppUser>().AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();
        serviceCollection.AddAuthentication(IdentityConstants.BearerScheme);
        serviceCollection.AddAuthorization();
        serviceCollection.Configure<IdentityOptions>(options => { options.User.RequireUniqueEmail = true; });
        return serviceCollection;
    }
}
