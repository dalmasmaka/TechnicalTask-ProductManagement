using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace PM_API.Exceptions
{
    public class CustomForbiddenResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomForbiddenResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var statusCode = httpContext.Response.StatusCode;

            // If status is Forbidden, modify the response
            if (statusCode == StatusCodes.Status403Forbidden)
            {
                httpContext.Response.ContentType = "application/json";
                var response = new
                {
                    message = "You do not have permission to access this resource."
                };
                await httpContext.Response.WriteAsJsonAsync(response);
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
