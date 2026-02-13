using Domain.Expctions;
using Microsoft.AspNetCore.Http;
using Shared.ErrorModel;

namespace Store_Api.MiddleWare
{
    public class GlobalErrorHandlingMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleWare> _logger;

        public GlobalErrorHandlingMiddleWare(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await HandlingNotFoundEndPointAsync(context);
            }
            catch (Exception ex)
            {
                await HandlingErrorAsync(context, ex);

            }
        }

        private async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new ErrorDetails()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMessage = ex.Message
            };

            response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                unAuthorizedException => StatusCodes.Status401Unauthorized,
                ValidationException => HandlingValidationExceptionAsync((ValidationException) ex, response),
                _ => StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = response.StatusCode;

            await context.Response.WriteAsJsonAsync(response);
        }

        private async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            await _next.Invoke(context);
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                context.Response.ContentType = "application/json";
                var response = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = $"End Point {context.Request.Path} Is Not Found"
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private int HandlingValidationExceptionAsync(ValidationException ex, ErrorDetails respons)
        {
            respons.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;
        }

    }
}
