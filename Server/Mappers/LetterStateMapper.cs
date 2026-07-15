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
                }).ToList();

            return stateDTO;
        }
    }
}
