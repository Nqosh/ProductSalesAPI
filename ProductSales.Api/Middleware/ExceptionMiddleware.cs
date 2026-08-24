using Microsoft.AspNetCore.Mvc;
using ProductSales.Infrastructure;

namespace ProductSales.Api.Middleware;

public class ExceptionMiddleware(
    RequestDelegate next,
    ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException)
            when (context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = 499;
        }
        catch (ExternalApiException ex)
        {
            logger.LogError(ex, "Assessment API integration failed");

            await WriteProblemDetailsAsync(
                context,
                StatusCodes.Status502BadGateway,
                "Assessment API unavailable",
                ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled request error");

            await WriteProblemDetailsAsync(
                context,
                StatusCodes.Status500InternalServerError,
                "Unexpected server error",
                "The request could not be completed.");
        }
    }

    private static Task WriteProblemDetailsAsync(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        context.Response.StatusCode = statusCode;

        return context.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            });
    }
}