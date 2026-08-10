using Domain.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Letter : BaseLetterModel
    {
        public User Addressee { get; set; }
        public Guid AddresseeId { get; set; }

        public List<User> Recipients { get; set; }

        public bool? Forwarded { get; set; }
        public User? ForwardRecipient { get; set; }
        public Guid? ForwardRecipientId { get; set; }
        public User? OriginalAuthor { get; set; }
        public Guid? OriginalAuthorId { get; set; }

        public DateTime SendTime { get; set; }

        public List<LetterState> LetterStates { get; set; }

        public List<Letter> ChildrenLetters { get; set; } = [];
        public Letter? ParentLetter { get; set; }
        public Guid? ParentLetterId { get; set; }

        public Letter()
        {
            Id = Guid.NewGuid();
        }

        public void AddChild(Letter childLetter)
        {
            ChildrenLetters.Add(childLetter);
        }
    }
}
