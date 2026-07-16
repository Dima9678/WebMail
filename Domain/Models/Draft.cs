using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Draft : BaseLetterModel
    {
        public User Author { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime LastEditDate { get; set; }
        public Draft()
        {
            Id = Guid.NewGuid();
        }
    }
}
