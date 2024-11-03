using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Support.Helpers;

namespace Support.Authorization.Requirements;

public class SupportUserRequirement(ILogger<SupportUserRequirement> logger) :
    AuthorizationHandler<SupportUserRequirement, HubInvocationContext>,
    IAuthorizationRequirement
{
    public const string Policy = "SupportUsersOnly";

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        SupportUserRequirement requirement,
        HubInvocationContext resource)
    {
        var hubContext = (context.Resource as HubInvocationContext)?.Context;
        if (hubContext == null)
        {
            logger.LogError("Could not get hubContext for {resource}", context.Resource);
            return Task.CompletedTask;
        }

        if (new ChatHubContextItemsHelper(hubContext.Items).IsSupportUser)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
