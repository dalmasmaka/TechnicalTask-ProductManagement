using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PM_Domain.Entities;

namespace PM_Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            await CreateUserIfNotExists(userManager, "admin@example.com", "admin", "Admin usr", "Admin@123", "Admin");
            await CreateUserIfNotExists(userManager, "user@example.com", "user", "Nonadmin usr", "User@123", "User");
        }

        private static async Task CreateUserIfNotExists(UserManager<ApplicationUser> userManager, string email, string username, string fullName, string password, string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = fullName,
                };
                //string hashedPassword = Argon2PasswordHasher.HashPassword(password);
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user {email}: {error.Description}");
                    }
                }
            }
        }
    }
}
