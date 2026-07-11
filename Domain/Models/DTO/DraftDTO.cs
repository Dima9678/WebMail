using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.DTO
{
    public class DraftDTO : BaseLetterModel
    {
        public bool Starred { get; set; }
    }
}
