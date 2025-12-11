namespace Intec.Workshop1.Customers.Infrastructure.Exceptions;

using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

        var (statusCode, title, errors) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "One or more validation errors occurred",
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request",
                new Dictionary<string, string[]>()
            )
        };

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            title,
            status = statusCode,
            errors
        }, cancellationToken);

        return true;
    }
}