using System.ComponentModel.DataAnnotations;
using System.Net;
using Support.Exceptions;

namespace Support.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (HttpResponseException exception)
        {
            context.Response.StatusCode = (int)exception.ResponseStatusCode;
            await context.Response.WriteAsJsonAsync(new { message = exception.Message });
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(exception.Message);
        }
    }
}
