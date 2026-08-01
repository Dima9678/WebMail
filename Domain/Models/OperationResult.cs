using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class OperationResult
    {
        public bool Sucsessed { get; set; } 
        public string? ErrorMessage { get; set; }
    }
}
