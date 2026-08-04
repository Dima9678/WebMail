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

        public async Task<UserDTO> Get(Guid userId)
        {
            var user = await _db.Users
                .SingleOrDefaultAsync(u => u.Id == userId);

            UserDTO dto = UserMapper.ToShortDto(user);

            return dto;
        }
    }
}
