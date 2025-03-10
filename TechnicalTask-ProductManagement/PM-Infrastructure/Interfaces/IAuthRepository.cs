﻿using PM_Domain.Entities;

namespace PM_Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> UserExists(string username);
        Task<bool> Register(string email, string username, string password, string role);
        Task<bool> Login(string username, string password);
        Task<bool> Logout(string id);
        Task <IEnumerable<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string id);
        Task<bool>UpdateUser(ApplicationUser user);

        Task<bool>DeleteUser(string id);
        Task<int> GetTotalUsersCountAsync();
    }
}
