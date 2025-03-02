using Microsoft.AspNetCore.Identity;
using PM_Application.DTOs;
using PM_Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO> UserExists(string username);
        Task<ResponseDTO> Register(string email, string username, string password, string confirmPassword, string role);
        Task<AuthResponseDTO> Login(string username, string password);
        Task<ResponseDTO> Logout(string id);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(string id);
    }
}
