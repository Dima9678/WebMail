using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Requests
{
    public class NewDraftDTO
    {
        public string? RecipientEmail { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? DraftId { get; set; }
    }
}
