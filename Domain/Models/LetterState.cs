using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class LetterState
    {
        public Guid Id { get; set; }
        public Guid LetterId { get; set; }
        public Letter Letter { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public bool Starred { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRead { get; set; }

        public LetterState() 
        {
            Id = Guid.NewGuid();
            IsDeleted = false;
            Starred = false;
        }

    }
}
