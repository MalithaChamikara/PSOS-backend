using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PSOS_ApplicationService.Exceptions;

namespace PSOS_Api.Middleware
{
    public class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
        {
            var (status, title, detail) = exception switch
            {
                ValidationException e => (StatusCodes.Status400BadRequest, "Invalid request", e.Message),
                UnauthorizedLoginException e => (StatusCodes.Status401Unauthorized, "Login failed", e.Message),
                InsufficientStockException e => (StatusCodes.Status409Conflict, "Insufficient stock", e.Message),
                _ => (StatusCodes.Status500InternalServerError, "Server error",
                                                   "An unexpected error occurred.")
            };

            if (status == StatusCodes.Status500InternalServerError)
                logger.LogError(exception, "Unhandled exception");

            var problem = new ProblemDetails { Status = status, Title = title, Detail = detail };

            if (exception is InsufficientStockException stock)
                problem.Extensions["shortages"] = stock.Shortages;   

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problem, ct);
            return true;
        }
    }
}
