using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTO
{
    public class LetterStateDTO
    {
        public Guid Id { get; set; }
        public Guid LetterId { get; set; }
        public LetterDTO Letter { get; set; }
        public Guid UserId { get; set; }
        public UserDTO User { get; set; }
        public bool Starred { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsRead { get; set; }
    }
}
