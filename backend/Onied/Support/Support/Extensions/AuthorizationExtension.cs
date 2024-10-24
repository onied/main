using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Support.Authorization.Requirements;

namespace Support.Extensions;

public static class AuthorizationExtension
{
    public static AuthenticationBuilder AddAuthorizationConfiguration(this IServiceCollection services)
    {
        var logger = services.BuildServiceProvider().GetService<ILogger<SupportUserRequirement>>();
        return services
            .AddAuthorization(options =>
                               {
                                   options.AddPolicy(SupportUserRequirement.Policy, policy =>
                                   {
                                       policy.Requirements.Add(new SupportUserRequirement(logger!));
                                   });
                               })
            .AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            .AddNegotiate();
    }
}
