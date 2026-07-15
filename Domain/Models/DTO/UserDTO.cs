using Domain.Models;
using Domain.Models.DTO;

namespace Domain
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<LetterDTO> SentLetters { get; set; } = new List<LetterDTO>();
        public List<LetterDTO> AcceptLetters { get; set; } = new List<LetterDTO>();
        public List<LetterStateDTO> LetterStates { get; set; } = new List<LetterStateDTO>();
        public List<DraftDTO> Drafts { get; set; }
    }
}
