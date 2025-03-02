using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PM_Application.Authorization;
using PM_Application.DTOs;
using PM_Application.DTOs.User;
using PM_Application.Interfaces;
using PM_Domain.Entities;
using PM_Infrastructure.Interfaces;


namespace PM_Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly JwtHelper _jwtHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IAuthRepository authRepository, IMapper mapper, ILogger<AuthService> logger, JwtHelper jwtHelper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _logger = logger;
            _jwtHelper = jwtHelper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                var users = await _authRepository.GetAllUsers();
                if (!users.Any())
                {
                    _logger.LogWarning("No users found.");
                    throw new KeyNotFoundException("No users found.");
                }
                return _mapper.Map<IEnumerable<UserDTO>>(users);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new KeyNotFoundException(ex.Message);
            }
        }
        public async Task<UserDTO> GetUserById(string id)
        {
            try
            {
                var user = _authRepository.GetUserById(id);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    throw new KeyNotFoundException("User not found.");
                }
                return _mapper.Map<UserDTO>(user);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new KeyNotFoundException("Errot fetching the user details");
            }
        }

        public async Task<AuthResponseDTO> Login(string username, string password)
        {
            try {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    _logger.LogWarning("Username or password is empty.");
                    throw new KeyNotFoundException("Username or password is empty.");
                }
                var result = await _authRepository.Login(username, password);
                if (!result)
                {
                    return new AuthResponseDTO { Success = true, Message = "User log in failed." };
                }
                var user = await _userManager.FindByNameAsync(username);
                if (user == null) 
                {
                    return new AuthResponseDTO { Success = false, Message = "User not found." };
                }
                var roles = await _userManager.GetRolesAsync(user);
                string userRole = roles.FirstOrDefault() ?? "User";
                var token = _jwtHelper.GenerateToken(username, password, userRole); 
                return new AuthResponseDTO { Success = true, Message = "User logged in successfully.", Token = token };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new AuthResponseDTO { Success = true, Message = "An error occurred while logging in. Please try again later."};
            }
        }

        public async Task<ResponseDTO> Logout(string id)
        {
            try
            {
                if(string.IsNullOrEmpty(id))
                {
                    _logger.LogWarning("User id is empty.");
                    throw new KeyNotFoundException("User id is empty.");
                }

                var result = await _authRepository.Logout(id);
                if (!result)
                {
                    _logger.LogWarning("User logout failed.");
                    return new ResponseDTO { Success = false, Message = "User logout failed." };
                }
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext != null)
                {
                    httpContext.Session.Clear();
                }
                return new ResponseDTO { Success = true, Message = "User logged out." };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseDTO { Success = false, Message = "An error occurred while logging out. Please try again later." };
            }
        }

        public async Task<ResponseDTO> Register(string email, string username, string password, string confirmPassword, string role)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    _logger.LogWarning("Email, username or password is empty.");
                    throw new ArgumentException("Email, username or password cannot be empty.");
                }

                if (password != confirmPassword)
                {
                    _logger.LogWarning("Passwords do not match.");
                    throw new ArgumentException("Passwords do not match.");
                }
                var userExistsResponse = await UserExists(username);
                if (userExistsResponse.Success)
                {
                    _logger.LogWarning("Username already taken.");
                    return new ResponseDTO { Success = false, Message = "Username is already taken. Please choose another one." };
                }
                var result = await _authRepository.Register(email, username, password, role);
                if (!result)
                {
                    _logger.LogWarning("User registration failed.");
                    return new ResponseDTO { Success = false, Message = "User registration failed. Please try again." };
                }

                return new ResponseDTO { Success = true, Message = "User registered successfully." };
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                return new ResponseDTO { Success = false, Message = ex.Message }; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseDTO { Success = false, Message = "An error occurred while registering. Please try again later." }; 
            }
        }

        public async Task<ResponseDTO> UserExists(string username)
        {
            try
            {
                if(string.IsNullOrEmpty(username))
                {
                    _logger.LogWarning("Username is empty.");
                    throw new KeyNotFoundException("Username is empty.");
                }
                var result = await  _authRepository.UserExists(username);
                if (result) {
                    return new ResponseDTO { Success = true, Message = "User exists. Try another username." };
                }
                return new ResponseDTO { Success = false };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ArgumentException("An error occurred while checking if the user exists. Please try again later.");
            }
        }
    }
}
