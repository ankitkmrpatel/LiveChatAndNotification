using NotificationBackendService.Data.Exceptions;
using System.Net;

namespace NotificationBackendService.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadRequestException badRequestException)
        {
            await GenerateError(context, HttpStatusCode.BadRequest, badRequestException.Message);
        }
        catch (NotFoundException notFoundExceptions)
        {
            await GenerateError(context, HttpStatusCode.NotFound, notFoundExceptions.Message);
        }
        catch (ConflictException conflictException)
        {
            await GenerateError(context, HttpStatusCode.Conflict, conflictException.Message);
        }
        catch (Exception exception)
        {
            Console.WriteLine("[INTERNAL-ERROR]" + exception.Message);
            Console.WriteLine("[INTERNAL-ERROR]" + exception);

            var message = "Internal Error on the Server.";
            await GenerateError(context, HttpStatusCode.InternalServerError, message);
        }
    }

    private static async Task GenerateError(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;
        var error = new ApplicationError
        {
            message = message,
            date = new DateTime()
        };
        await context.Response.WriteAsJsonAsync(error);
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}
