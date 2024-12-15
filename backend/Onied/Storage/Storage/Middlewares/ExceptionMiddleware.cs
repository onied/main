using System.ComponentModel.DataAnnotations;
using System.Net;
using Storage.Exceptions;

namespace Storage.Middlewares;

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
            await context.Response.WriteAsJsonAsync(new { detail = exception.Message });
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new { detail = exception.Message });
        }
    }
}
