using Domain;
using Domain.Models;
using Domain.Intefraces;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Server.Mappers;

namespace Server.Service
{
    public class UserService : IUserService
    {
        private DatabaseContext _db { get; set; }
        public UserService(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<UserDTO> Get(Guid userId)
        {
             var user = await _db.Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            UserDTO dto = UserMapper.ToShortDto(user);

            return dto;
        }

        public async Task ChangeUserInfo(UserDataChangeRequest request, Guid userId)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            user.Surname = request.Surname;
            user.Email = request.Email;
            user.Name = request.Name;
            user.IsMan = request.IsMan;

            await _db.SaveChangesAsync();
        }

        public async Task AddSpamEmail(Guid userId, string adresseeEmail)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (!user.SpamEmails.Contains(adresseeEmail))
            {
                user.SpamEmails.Add(adresseeEmail);
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveSpamEmail(Guid userId, string adresseeEmail)
        {
            User user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.SpamEmails.Remove(adresseeEmail);

            await _db.SaveChangesAsync();
        }
    }
}
