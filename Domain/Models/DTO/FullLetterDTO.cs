using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTO
{
    public class FullLetterDTO : LetterDTO
    {
        public Guid? PreviousLetterId { get; set; }
        public Guid? NextLetterId { get; set; }
        public int LetterNumber { get; set; }
    }
}
