using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class DraftDTO : BaseLetterModel
    {
        public bool Starred { get; set; }
    }
}
