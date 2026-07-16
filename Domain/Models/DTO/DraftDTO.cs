using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTO
{
    public class DraftDTO : BaseLetterModel
    {
        public UserDTO Author { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime LastEditDate { get; set; }
    }
}
