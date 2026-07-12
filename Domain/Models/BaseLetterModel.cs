using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class BaseLetterModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public bool Starred { get; set; }
    }
}
