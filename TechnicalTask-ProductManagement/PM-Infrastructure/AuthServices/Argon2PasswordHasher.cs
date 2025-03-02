using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using PM_Domain.Entities;
using System;
using System.Text;

namespace PM_Infrastructure.AuthServices
{
    public class Argon2PasswordHasher : IPasswordHasher<ApplicationUser>
    {
        public string HashPassword(ApplicationUser user, string password)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.DegreeOfParallelism = 8;  // Controls parallelism (threads)
            argon2.MemorySize = 65536;  // Memory cost (in KB)
            argon2.Iterations = 4;  // Number of iterations

            byte[] hash = argon2.GetBytes(32);  // Output length (in bytes)
            return Convert.ToBase64String(hash);  // Return base64-encoded hash
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            string providedPasswordHash = HashPassword(user, providedPassword);

            if (providedPasswordHash == hashedPassword)
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}
