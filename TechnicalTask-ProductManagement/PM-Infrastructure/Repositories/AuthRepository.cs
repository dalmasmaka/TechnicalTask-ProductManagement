using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; 
using Microsoft.EntityFrameworkCore;
using PM_Infrastructure.Interfaces;
using PM_Infrastructure.AuthServices;
using PM_Infrastructure.Data;
using PM_Domain.Entities;

namespace PM_Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager; // ASP.NET Identity UserManager
        private readonly SignInManager<ApplicationUser> _signInManager; // SignInManager for handling login
        private readonly ApplicationDbContext _context;

        public AuthRepository(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<bool> UserExists(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<bool> Register(string email, string username, string password, string role)
        {

            //string hashedPassword = Argon2PasswordHasher.HashPassword(password);

            var user = new ApplicationUser
            {
                Email = email,
                UserName = username,
                FullName = username,
                PasswordHash = password,
                
            };

            var result = await _userManager.CreateAsync(user, password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
                return true;
            }
            return false;
        }
        public async Task<bool> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return false;
            }

            var passwordVerificationResult = new Argon2PasswordHasher().VerifyHashedPassword(user, user.PasswordHash, password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                user.LoginTimestamp = DateTime.UtcNow; 
                await _userManager.UpdateAsync(user);
                return true;
            }

            return false;
        }



        public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return await _userManager.Users.ToListAsync();
          
        }
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> Logout(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            user.LogOutTimestamp = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;

        }
    }
}
