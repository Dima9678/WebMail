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
        public bool IsReaden { get; set; }
        public DateTime SendTime { get; set; }
    }
}
