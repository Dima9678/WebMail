using Domain;
using Domain.Models;
using Domain.Models.DTO;
using Server.Controllers;

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
            }).ToList();

            return letterDTO;
        }
        public static LetterDTO ToDto(Letter letterInDb)
        {
            LetterDTO letterDTO = new LetterDTO()
            {
                Id = letterInDb.Id,
                AdresseeName = letterInDb.Addressee.Name,
                AdresseeSurname = letterInDb.Addressee.Surname,
                AdresseeEmail = letterInDb.Addressee.Email,
                LetterStates = LetterStateMapper.StateListToDTO(letterInDb.LetterStates),
                Title = letterInDb.Title,
                Text = letterInDb.Text,
                SendTime = letterInDb.SendTime,
            };
            return letterDTO;
        }
        public static FullLetterDTO ToFullDto(Letter letterInDb)
        {
            FullLetterDTO letterDTO = new FullLetterDTO()
            {
                Id = letterInDb.Id,
                AdresseeName = letterInDb.Addressee.Name,
                AdresseeSurname = letterInDb.Addressee.Surname,
                AdresseeEmail = letterInDb.Addressee.Email,
                LetterStates = LetterStateMapper.StateListToDTO(letterInDb.LetterStates),
                Title = letterInDb.Title,
                Text = letterInDb.Text,
                SendTime = letterInDb.SendTime,
                RecipientId = letterInDb.RecipientId,
                ParentLetterId = letterInDb.ParentLetterId,
                ChildrenLetters = LetterMapper.ListToDTO(letterInDb.ChildrenLetters)
            };
            //на случай, если нет родительского письма
            if (letterInDb.ParentLetter == null)
            {
                letterDTO.ParentLetter = null;
            }
            else
            {
                letterDTO.ParentLetter = LetterMapper.ToDto(letterInDb.ParentLetter);
            }

            return letterDTO;
        }

        public static NewLetterDTO DraftDTOToLetterDTO(NewDraftDTO draft)
        {
            NewLetterDTO letter = new NewLetterDTO()
            {
                Recipient = draft.RecipientEmail,
                Title = draft.Title,
                Text = draft.Text,
            };
            return letter;
        }

        public static List<LetterDTO> ListToDTO(List<Letter> letterInDb)
        {
            List<LetterDTO> list = new List<LetterDTO>(); 
            foreach (Letter letter in letterInDb)
            {
                LetterDTO letterDTO = new LetterDTO()
                {
                    Id = letter.Id,
                    AdresseeName = letter.Addressee.Name,
                    AdresseeSurname = letter.Addressee.Surname,
                    AdresseeEmail = letter.Addressee.Email,
                    Title = letter.Title,
                    Text = letter.Text,
                    SendTime = letter.SendTime,
                };
                list.Add(letterDTO);
            }
            return list;
        }
    }
}
