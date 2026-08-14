using Domain;
using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;

namespace Server.Mappers
{
    public class LetterMapper
    {
        public static LetterDTO ToDto(Letter letterInDb)
        {
            LetterDTO letterDTO = new LetterDTO()
            {
                Id = letterInDb.Id,
                AdresseeName = letterInDb.Addressee.Name,
                AdresseeSurname = letterInDb.Addressee.Surname,
                AdresseeEmail = letterInDb.Addressee.Email,
                LetterStates = LetterStateMapper
                    .StateListToDTO(letterInDb.LetterStates),
                Title = letterInDb.Title,
                Text = letterInDb.Text,
                SendTime = letterInDb.SendTime,
            };
            return letterDTO;
        }
        public static List<LetterDTO> ToDTO(List<Letter> letterInDb)
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
        public static FullLetterDTO ToFullDto(Letter letterInDb)
        {
            FullLetterDTO letterDTO = new FullLetterDTO()
            {
                Id = letterInDb.Id,
                AdresseeName = letterInDb.Addressee.Name,
                AdresseeSurname = letterInDb.Addressee.Surname,
                AdresseeEmail = letterInDb.Addressee.Email,
                LetterStates = LetterStateMapper.StateListToDTO(letterInDb.LetterStates),
                Forwarded = letterInDb.Forwarded,
                Title = letterInDb.Title,
                Text = letterInDb.Text,
                SendTime = letterInDb.SendTime,
                ParentLetterId = letterInDb.ParentLetterId,
                Recipients = UserMapper.ToShortDto(letterInDb.Recipients),
                ChildrenLetters = LetterMapper.ToDTO(letterInDb.ChildrenLetters)
            };
            if (letterInDb.Forwarded == true)
            {
                letterDTO.OriginalAuthor = UserMapper.ToShortDto(letterInDb.OriginalAuthor);
            }
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
                Recipients = draft.Recipients,
                Title = draft.Title,
                Text = draft.Text,
            };
            return letter;
        }
    }
}
