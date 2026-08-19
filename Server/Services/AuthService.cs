using Domain;
using Domain.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

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

        public async Task<User> RegisterAsync(RegisterDTO dto)
        {
            User user = new User()
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                IsMan = dto.IsMan,
                PasswordHash = _hasher.HashPassword(new User(), dto.Password),
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }
        public async Task<User> LoginAsync(LoginDTO dto)
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
