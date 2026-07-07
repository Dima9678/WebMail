using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Draft : BaseLetterModel
    {
        public bool Starred { get; set; }

        public Draft()
        {
            Id = Guid.NewGuid();
        }
    }
}
