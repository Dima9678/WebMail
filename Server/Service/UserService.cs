using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Server.Mappers;

namespace Server.Service
{
    public class UserService
    {
        private DatabaseContext _db { get; set; }
        public UserService(DatabaseContext db)
        {
            _db = db;
        }

        public UserDTO Get(string userId)
        {

            var user = _db.Users
     .Include(u => u.SentLetters)
         .ThenInclude(l => l.Recipient)
     .Include(u => u.AcceptLetters)
         .ThenInclude(l => l.Addressee)
     .SingleOrDefaultAsync(u => u.Id == Guid.Parse(userId));

            UserDTO dto = UserMapper.ToDto();

            UserDTO dto = new UserDTO()
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,

                SentLetters = user.SentLetters.Select(l => new LetterDTO
                {
                    Id = l.Id,
                    Title = l.Title,
                    Text = l.Text,
                    SendTime = l.SendTime,
                    RecipientId = l.RecipientId,
                    AddresseeId = l.AddresseeId,
                    IsReaden = l.IsReaden,
                    Starred = l.Starred
                }).ToList(),

                AcceptLetters = user.AcceptLetters.Select(l => new LetterDTO
                {
                    Id = l.Id,
                    Title = l.Title,
                    Text = l.Text,
                    SendTime = l.SendTime,
                    RecipientId = l.RecipientId,
                    AddresseeId = l.AddresseeId,
                    AdresseeName = l.Addressee.Name,
                    IsReaden = l.IsReaden,
                    Starred = l.Starred
                }).ToList()
            };

            return dto;
        }
    }
}
