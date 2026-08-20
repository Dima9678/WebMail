using Domain.Models;
using Domain.Models.DTO;
using Domain.Models.Requests;

namespace Domain.Intefraces
{
    public interface ILetterService
    {
        Task CreateLetterAsync(NewLetterDTO request, Guid adresseeId, string[] recipients);
        Task CreateReplyAsync(ReplyDTO replyText, Guid adresseeId, Guid parentLetterId);
        Task<OperationResult> ForwardAsync(ForwardRequest request, Guid userId);

        Task<FullLetterDTO> GetByIdAsync(Guid letterId, Guid userId, string from);

        Task<List<LetterDTO>> GetAcceptLettersAsync(Guid userId, int startIndex, int endIndex);
        Task<List<LetterDTO>> GetSentLettersAsync(Guid userId, int startIndex, int endIndex);
        Task<List<LetterDTO>> GetStarredLettersAsync(Guid userId, int startIndex, int endIndex);
        Task<List<LetterDTO>> GetSpamLettersAsync(Guid userId, int startIndex, int endIndex);

        Task ChangeStarredAsync(Guid letterId, Guid userId);
        Task ChangeIsReadenAsync(Guid userId, Letter letterInDb);
        Task ChangeIsReadenAsync(Guid letterId, Guid userId);
        Task ChangeIsSpamAsync(Guid letterId, Guid userId);

        Task<int> GetAcceptCountAsync(Guid userId);
        Task<int> GetSendCountAsync(Guid userId);
        Task<int> GetStarredCountAsync(Guid userId);
        Task<int> GetSpamCountAsync(Guid userId);

        Task<FullLetterDTO> AppendAcceptNavigationInfoAsync(FullLetterDTO fullLetter, Guid userId);
        Task<FullLetterDTO> AppendSentNavigationInfoAsync(FullLetterDTO fullLetter, Guid userId);
        Task<FullLetterDTO> AppendStarredNavigationInfoAsync(FullLetterDTO fullLetter, Guid userId);
    }
}