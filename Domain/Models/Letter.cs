using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Letter : BaseLetterModel
    {
        public User Addressee { get; set; }
        public Guid AddresseeId { get; set; }
        public User Recipient { get; set; }
        public Guid RecipientId { get; set; }
        public DateTime SendTime { get; set; }
        public List<LetterState> LetterStates { get; set; }

        public Letter()
        {
            Id = Guid.NewGuid();
        }
    }
}
