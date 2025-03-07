using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PM_Application.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PM_API.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;

        public JwtMiddleware(IOptions<JwtSettings> jwtSettings, RequestDelegate next)
        {
            _jwtSettings = jwtSettings.Value;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth/login") || context.Request.Path.StartsWithSegments("/api/auth/register"))
            {
                await _next(context);
                return;
            }
            var authorizationHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Authorization header is missing.");
                return;
            }

            var token = authorizationHeader.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Token is missing.");
                return;
            }

            if (!ValidateToken(context, token))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid or expired token.");
                return;
            }

            await _next(context);
        }
        public bool ValidateToken(HttpContext context, string token)
        {

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
                };

                var principal = handler.ValidateToken(token, validationParameters, out var validatedToken);
                context.User = principal;

                // Optionally check if the token is of the expected type (JWT)
                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    throw new SecurityTokenException("Invalid token algorithm.");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Token validation failed.", ex);
            }
        }

    }
}
