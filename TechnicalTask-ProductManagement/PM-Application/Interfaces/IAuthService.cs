using PM_Application.DTOs;
using PM_Application.DTOs.User;

namespace PM_Application.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO> UserExists(string username);
        Task<ResponseDTO> Register(string email, string username, string password, string role);
        Task<AuthResponseDTO> Login(string username, string password);
        Task<ResponseDTO> Logout(string id);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(string id);
        Task<UpdateUserDto> UpdateUser(UpdateUserDto user);
        Task<ResponseDTO> DeleteUser(string id);
        Task<int> GetTotalUsersCountAsync();
    }
}
