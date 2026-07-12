using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Server.Controllers;

namespace Server.Service
{
    public class AuthService
    {
        private DatabaseContext _db { get; set; }
        private PasswordHasher<User> _hasher = new();
        public AuthService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<User> Register(RegisterDTO dto)
        {
            User user = new User()
            {
                Name = dto.Name,
                Email = dto.Email,
            };
            string hashedPassword = _hasher.HashPassword(new User(), dto.Password);

            user.PasswordHash = hashedPassword;

            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }
        public async Task<User> Login(LoginDTO dto)
        {
            User? userInDb = await _db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);

            var result = _hasher.VerifyHashedPassword(
            userInDb,
            userInDb.PasswordHash,
            dto.Password);

            if (result == PasswordVerificationResult.Success)
            {
                return userInDb;
            }
            else
            {
                return null;
            }
        }
    }
}
