using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PixelV0.Shared.Exceptions;

namespace PixelV0.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred while processing the request. Message: {Message}", exception.Message);
            var details = new ProblemDetails
            {
                Instance = httpContext.Request.Path,
            };

            switch (exception)
            {
                case InvalidCredentialsException:
                    details.Title = "Invalid credentials";
                    details.Status = StatusCodes.Status401Unauthorized;
                    break;
                case UserAlreadyExistsException:
                    details.Title = "User already exists";
                    details.Status = StatusCodes.Status409Conflict;
                    break;
                case UserNotFoundException:
                    details.Title = "User not found";
                    details.Status = StatusCodes.Status404NotFound;
                    break;
                case NotAuthorizedException:
                    details.Title = "Not authorized";
                    details.Status = StatusCodes.Status403Forbidden;
                    break;
                default:
                    details.Title = "An unexpected error occurred";
                    details.Status = StatusCodes.Status500InternalServerError;
                    break;
            }
            await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
            return true;
        }
    }
}
