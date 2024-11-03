using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Support.Abstractions;

namespace Support.Authorization.Filters;

public class AuthorizeSupportUserAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("userId", out var userIdObject) || userIdObject == null)
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails
            {
                Errors = { { "userId", new[] { "userId query parameter cannot be null" } } }
            });
            return;
        }

        Guid userId;
        if (userIdObject is Guid guid)
            userId = guid;
        else if (!Guid.TryParse((string?)userIdObject, out userId))
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails
            {
                Errors = { { "userId", new[] { "userId must be a valid guid." } } }
            });
            return;
        }

        var authorizationSupportUserService = context.HttpContext.RequestServices.GetService<IAuthorizationSupportUserService>();

        if (authorizationSupportUserService == null)
            throw new NullReferenceException($"No service added {nameof(IAuthorizationSupportUserService)}");

        await authorizationSupportUserService.AuthorizeSupportUser(userId);
        await next();
    }
}
