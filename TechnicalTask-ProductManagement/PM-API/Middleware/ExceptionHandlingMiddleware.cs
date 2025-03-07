using System.Net;
using System.Text.Json;

namespace PM_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new { message = exception.Message };
            response.StatusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound, //404 not found errors
                ApplicationException => (int)HttpStatusCode.BadRequest, // 400 bad request errors  
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(errorResponse);
            await response.WriteAsync(result);
        }
    }
}
//kthen json me mesazhin e gabimit dhe status code te duhur apo dinamik ne rast se ndodh nje gabim i paparashikuar
