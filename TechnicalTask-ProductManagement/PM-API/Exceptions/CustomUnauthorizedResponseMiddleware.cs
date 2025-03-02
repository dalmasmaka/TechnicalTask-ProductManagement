namespace PM_API.Exceptions
{
    public class CustomUnauthorizedResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomUnauthorizedResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var statusCode = httpContext.Response.StatusCode;

            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                httpContext.Response.ContentType = "application/json";
                var response = new
                {
                    message = "You are not authorized. Please provide a valid JWT token."
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
