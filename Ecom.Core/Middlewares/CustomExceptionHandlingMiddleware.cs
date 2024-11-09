using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ecom.Core.Middlewares;
public class CustomExceptionHandlingMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleGlobalExceptionAsync(context, ex);
        }
    }

    private Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception is ApplicationException)
        {
            logger.LogWarning("Validation error occurred. {message}", exception.Message);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return context.Response.WriteAsJsonAsync(new { exception.Message });
        }
        else
        {
            var errorId = Guid.NewGuid();
            logger.LogError(exception, "Error occurred in API: {ErrorId}", errorId);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsJsonAsync(new
            {
                ErrorId = errorId,
                Message = "Oops! We were unable to process this request. Please try again!"
            });
        }
    }
}
