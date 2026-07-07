using Domain.Models;

namespace Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Letter> SentLetters { get; set; } = new List<Letter>();
        public List<Letter> AcceptLetters { get; set; } = new List<Letter>();

        public User() 
        {
            Id = Guid.NewGuid();
        }
    }
}
