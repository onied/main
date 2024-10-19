using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;

namespace Purchases.Extensions;

public static class AuthorizationExtension
{
    public static AuthenticationBuilder AddAuthorizationNegotiate(this IServiceCollection services)
    {
        return services
            .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();
    }
}
