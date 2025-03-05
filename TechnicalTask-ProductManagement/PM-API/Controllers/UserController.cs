using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PM_Application.DTOs;
using PM_Application.DTOs.User;
using PM_Application.Interfaces;
using System.Security.Claims;

namespace PM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<UserController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IAuthService authService, ILogger<UserController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            var users = await _authService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetById(string id)
        {
            var user = await _authService.GetUserById(id);
            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateUserDto>> Update(string id, UpdateUserDto user)
        {
            var updatedUser = await _authService.UpdateUser(user);
            return Ok(updatedUser);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseDTO>> Delete(string id)
        {
            var result = await _authService.DeleteUser(id);
            return Ok(result);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetTotalUsersCountAsync()
        {
            var result = await _authService.GetTotalUsersCountAsync();
            return Ok(result);
        }
    }

}
