using Domain.Models;
using Domain.Models.DTO;

namespace Server.Mappers
{
    public static class LetterStateMapper
    {
        public static List<LetterStateDTO> StateListToDTO(List<LetterState> letterStates)
        {
            List<LetterStateDTO> stateDTO = letterStates
                .Select(l => new LetterStateDTO
                {
                    Id = l.Id,
                    LetterId = l.LetterId,
                    UserId = l.UserId,
                    Starred = l.Starred,
                    IsDeleted = l.IsDeleted,
                    IsRead = l.IsRead,
                    IsSpam = l.IsSpam,
                }).ToList();

            return stateDTO;
        }
        public static LetterStateDTO ListToOne(List<LetterState> letterStates)
        {
            if (letterStates != null && letterStates.Count > 0)
            {
                LetterStateDTO stateDTO = new()
                {
                    Id = letterStates[0].Id,
                    LetterId = letterStates[0].LetterId,
                    UserId = letterStates[0].UserId,
                    Starred = letterStates[0].Starred,
                    IsDeleted = letterStates[0].IsDeleted,
                    IsRead = letterStates[0].IsRead,
                    IsSpam = letterStates[0].IsSpam,
                };
                return stateDTO;
            }
            else
            {
                return null;
            }
        }
    }
}
