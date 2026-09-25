using ConferenceBooking.BLL.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ConferenceBooking.API.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger): IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, $"Unhandled exception occurred: {exception.Message}");

        var (statusCode, title, detail, errors) = exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Validation Error",
                "One or more validation failures occurred.",
                validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })),

            NotFoundException notFoundException => (
                StatusCodes.Status404NotFound,
                "Not Found",
                notFoundException.Message,
                null),
            
            ConflictException conflictException => (
                StatusCodes.Status409Conflict,
                "Conflict",
                conflictException.Message,
                null),
            
            BadRequestException badRequestException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                badRequestException.Message,
                null),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Server Error",
                "An unexpected error occurred",
                null)
        };

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        if (errors is not null)
        {
            problemDetails.Extensions["errors"] = errors;
        }
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}