using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Task = System.Threading.Tasks.Task;

namespace Courses.Filters;

public class AllowVisitCourseValidationFilterAttribute : ActionFilterAttribute
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

        if (!context.ActionArguments.TryGetValue("role", out var role))
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails
            {
                Errors = { { "role", new[] { "role query parameter cannot be null" } } }
            });
            return;
        }

        Guid userId;
        if (userIdObject is Guid guid)
            userId = guid;
        else if (!Guid.TryParse((string?)userIdObject, out userId))
        {
            context.Result = new BadRequestResult();
            return;
        }

        var id = (int)context.ActionArguments["id"]!;

        var courseManagementService = context.HttpContext.RequestServices.GetService<ICourseManagementService>();

        if (courseManagementService == null)
            throw new NullReferenceException($"No service added {nameof(ICourseManagementService)}");

        if (!await courseManagementService.AllowVisitCourse(userId, id, (string?)role))
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}
