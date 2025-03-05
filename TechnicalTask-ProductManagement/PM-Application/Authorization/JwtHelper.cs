using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PM_Application.Authorization
{
    public class JwtHelper
    {
        private readonly JwtSettings _jwtSettings;

        public JwtHelper(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        }

        public string GenerateToken(string userId, string username, string email, string fullName, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId), // Use standard "sub"
                new Claim("username", username),
                new Claim("email", email),
                new Claim("role", role),
                new Claim("fullName", fullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.TokenValidityMins)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
