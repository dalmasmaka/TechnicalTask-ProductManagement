////using Microsoft.Extensions.Options;
////using PM_Application.Authorization;
////using System.IdentityModel.Tokens.Jwt;
////using System.Net;
////using System.Security.Claims;
////using System.Text.Json;
////using System.Text.Json.Serialization;

////namespace PM_API.Middleware
////{
////    public class AuthMiddleware
////    {
////        private readonly RequestDelegate _next;
////        private readonly IHttpContextAccessor httpContextAccessor;

////        public AuthMiddleware(RequestDelegate next,IHttpContextAccessor httpContextAccessor ) // Fix constructor name
////        {
////            _next = next;
////            this.httpContextAccessor = httpContextAccessor;
////        }

////        public async Task InvokeAsync(HttpContext context)
////        {
////            var authorizationHeader = context.Request.Headers["Authorization"].ToString();
////            Console.WriteLine($"Authorization Header: {authorizationHeader}");
////            if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/auth/register"))
////            {
////                await _next(context);
////                return;
////            }

////            //if (context.Response.HasStarted)
////            //{
////            //    return;
////            //}

////            var identity = context.User?.Identity;
////            if (identity == null || !identity.IsAuthenticated)
////            {
////                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
////                await WriteJsonResponseAsync(context, false, "Unauthorized: Please log in first.", HttpStatusCode.Unauthorized);
////                return;
////            }

////            await _next(context); // Proceed to next middleware only if authenticated

////            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
////            {
////                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
////                await WriteJsonResponseAsync(context, false, "You do not have permission to access this resource.", HttpStatusCode.Forbidden);
////            }
////        }
////        private async Task WriteJsonResponseAsync<T>(HttpContext context, T success, string message, HttpStatusCode statusCode)
////        {
////            context.Response.ContentType = "application/json";
////            context.Response.StatusCode = (int)statusCode;
////            var response = new { success, message };
////            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
////        }
////    }
////}
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using PM_Application.Authorization;
//using System.IdentityModel.Tokens.Jwt;
//using System.Net;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Json;

//namespace PM_API.Middleware
//{
//    public class AuthMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public AuthMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            // await _next(context);
//            var authorizationHeader = context.Request.Headers["Authorization"].ToString();
//            Console.WriteLine($"Authorization Header: {authorizationHeader}");
//            if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/auth/register"))
//            {
//                await _next(context);
//                return;
//            }


//            var identity = context.User?.Identity;
//            if (identity == null || !identity.IsAuthenticated)
//            {
//                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
//                await WriteJsonResponseAsync(context, false, "Unauthorized: Please log in first.", HttpStatusCode.Unauthorized);
//                return;
//            }
//            await _next(context);

//            if (context.Response.HasStarted)
//            {
//                return;
//            }
//            if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
//            {
//                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
//                await WriteJsonResponseAsync(context, false, "You do not have permission to access this resource.", HttpStatusCode.Forbidden);
//                return;
//            }
//        }

//        private async Task WriteJsonResponseAsync<T>(HttpContext context, T success, string message, HttpStatusCode statusCode)
//        {
//            context.Response.ContentType = "application/json";
//            context.Response.StatusCode = (int)statusCode;
//            var response = new { success, message };
//            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
//        }
//    }
//}
