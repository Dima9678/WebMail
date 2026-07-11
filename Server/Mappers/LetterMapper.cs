using Domain;
using Domain.Models;
using Domain.Models.DTO;

namespace Server.Mappers
{
    public class LetterMapper
    {
        public static List<LetterDTO> SentToDto(User user)
        {
            List<LetterDTO> letterDTO = user.SentLetters.Select(l => new LetterDTO
            {
                Id = l.Id,
                Title = l.Title,
                Text = l.Text,
                SendTime = l.SendTime,
                RecipientId = l.RecipientId,
                AddresseeId = l.AddresseeId,
                IsReaden = l.IsReaden,
                Starred = l.Starred
            }).ToList();

            return letterDTO;
        }
        public static List<LetterDTO> AcceptToDto(User user)
        {
            List<LetterDTO> letterDTO = user.AcceptLetters.Select(l => new LetterDTO
            {
                Id = l.Id,
                Title = l.Title,
                Text = l.Text,
                SendTime = l.SendTime,
                RecipientId = l.RecipientId,
                AddresseeId = l.AddresseeId,
                IsReaden = l.IsReaden,
                Starred = l.Starred
            }).ToList();

            return letterDTO;
        }
        public static LetterDTO ToDto(Letter letterInDb)
        {
            LetterDTO letterDTO = new LetterDTO()
            {
                Id = letterInDb.Id,
                AdresseeName = letterInDb.Addressee.Name,
                AdresseeEmail = letterInDb.Addressee.Email,
                IsReaden = letterInDb.IsReaden,
                Title = letterInDb.Title,
                Text = letterInDb.Text,
                SendTime = letterInDb.SendTime,
                Starred = letterInDb.Starred,
            };
            return letterDTO;
        }
        public static Letter toEntity(LetterDTO letterDTO)
        {
            return new Letter();
        }
    }
}
