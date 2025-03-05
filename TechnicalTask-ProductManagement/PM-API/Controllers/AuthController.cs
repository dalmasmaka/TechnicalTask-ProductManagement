using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM_Application.DTOs.User;
using PM_Application.Interfaces;
using System.Security.Claims;

namespace PM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(IAuthService authService, ILogger<AuthController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerDTO)
        {
            var result = await _authService.Register(registerDTO.Email, registerDTO.Username, registerDTO.Password, registerDTO.Role);
            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO user)
        {
            var result = await _authService.Login(user.Username, user.Password);
            return Ok(result);

        }
        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var result = await _authService.Logout(userId);
            return Ok(result);
        }
    }
    
}
