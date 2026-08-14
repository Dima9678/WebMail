using Domain.Models;

namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsMan { get; set; }
        public List<Letter> SentLetters { get; set; }
        public List<Letter> AcceptLetters { get; set; }
        public List<LetterState> LetterStates { get; set; }
        public List<Draft> Drafts { get; set; }
        public List<string>? SpamEmails { get; set; } = new();

        public User() 
        {
            Id = Guid.NewGuid();
        }
    }
}
