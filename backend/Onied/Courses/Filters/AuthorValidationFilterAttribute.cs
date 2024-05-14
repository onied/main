using Courses.Services;
using Courses.Services.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Task = System.Threading.Tasks.Task;

namespace Courses.Filters;

public class AuthorValidationFilterAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("userId", out object? userIdObject) || userIdObject == null)
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails
            {
                Errors = { { "userId", new[] { "userId query parameter cannot be null" } } }
            });
            return;
        }

        if (!context.ActionArguments.TryGetValue("role", out object? role))
        {
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails
            {
                Errors = { { "userId", new[] { "userId query parameter cannot be null" } } }
            });
            return;
        }

        if (!Guid.TryParse((string)userIdObject, out Guid userId))
        {
            context.Result = new BadRequestResult();
            return;
        }

        var id = (int)context.ActionArguments["id"]!;

        var courseManagementService = context.HttpContext.RequestServices.GetService<ICourseManagementService>();

        if (courseManagementService == null)
            throw new NullReferenceException($"No service added {nameof(ICourseManagementService)}");

        if (!await courseManagementService.AllowVisitCourse(userId, id))
        {
            context.Result = new ForbidResult();
            return;
        }

        var response = await courseManagementService.CheckCourseAuthorAsync(id, userId.ToString(), (string?)role);
        if (response is not Ok<string>)
        {
            context.Result = (IActionResult)response;
            return;
        }

        await next();
    }
}
