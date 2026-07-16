using Domain.Models.DTO;
using Domain.Models;

namespace Server.Mappers
{
    public static class DraftMapper
    {
        public static DraftDTO ToDTO(Draft draft)
        {
            DraftDTO dto = new DraftDTO()
            {
                Author = UserMapper.ToShortDto(draft.Author),
                AuthorId = draft.AuthorId,
                Id = draft.Id,
                Title = draft.Title,
                Text = draft.Text,
                LastEditDate = draft.LastEditDate,
            };

            return dto;
        }
    }
}
