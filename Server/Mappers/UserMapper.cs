using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Server.Mappers
{
    public class UserMapper
    {
        public static UserDTO ToDto(User user)
        {
            UserDTO dto = new UserDTO()
            {
                Name = user.Name,
                Email = user.Email,
                Id = user.Id,
                SentLetters = LetterMapper.SentToDto(user),
                AcceptLetters = LetterMapper.AcceptToDto(user),
            };
            
            return dto;
        }
        public static User ToEntity()
        {
            return new User();
        }
    }
}
