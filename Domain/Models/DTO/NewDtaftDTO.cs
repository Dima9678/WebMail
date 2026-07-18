using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTO
{
    public class NewDraftDTO
    {
        public string? RecipientEmail { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
    }
}
