using Domain.Models.DTO;
using Domain.Models.Requests;

namespace Domain.Intefraces
{
    public interface IDraftService
    {
        Task<DraftDTO> GetByIdAsync(Guid draftId);
        Task<List<DraftDTO>> GetUserDraftsAsync(Guid UserId, int startIndex, int endIndex);
        Task<int> GetTotalAcceptCountAsync(Guid userId);

        Task<Guid> AddDraftAsync(NewDraftDTO request, Guid authorId);
        Task DeleteDraftAsync(Guid draftId);
        Task SaveDraftAsync(NewDraftDTO request, Guid draftId);
    }
}