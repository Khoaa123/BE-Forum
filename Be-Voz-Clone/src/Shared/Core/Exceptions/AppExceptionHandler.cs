using Be_Voz_Clone.src.Services.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace Be_Voz_Clone.src.Shared.Core.Exceptions
{
    public class AppExceptionHandler : IExceptionHandler
    {
        private ILogger _logger;

        public AppExceptionHandler(ILogger<AppExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (int statusCode, string errorMessage, string errorCode) = exception switch
            {
                ForbiddenException forbiddenException => (403, forbiddenException.Message, "FORBIDDEN"),
                BadRequestException badRequestException => (400, badRequestException.Message, "BAD_REQUEST"),
                NotFoundException notFoundException => (404, notFoundException.Message, "NOT_FOUND"),
                _ => (500, exception.Message, "INTERNAL_SERVER_ERROR")
            };

            _logger.LogError(exception, exception.Message);
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse
            {
                StatusCode = statusCode,
                Code = errorCode,
                Message = errorMessage,
            });

            return true;
        }
    }
}
