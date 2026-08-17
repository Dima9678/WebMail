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
                Surname = user.Surname,
                Email = user.Email,
                Id = user.Id,
                SpamEmails = user.SpamEmails,
                SentLetters = LetterMapper.ToDTO(user.SentLetters),
                AcceptLetters = LetterMapper.ToDTO(user.AcceptLetters),
            };
            
            return dto;
        }
        public static UserDTO ToShortDto(User user)
        {
            UserDTO dto = new UserDTO()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Id = user.Id,
                IsMan = user.IsMan,
                SpamEmails = user.SpamEmails
            };
            
            return dto;
        }
        public static List<UserDTO> ToShortDto(List<User> users)
        {
            UserDTO[] dTOs = new UserDTO[users.Count];

            for (int i = 0; i < users.Count; i++)
            {
                dTOs[i] = ToShortDto(users[i]);
            }

            return dTOs.ToList();
        }
    }
}
