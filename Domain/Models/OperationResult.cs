using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class OperationResult
    {
        public bool Sucsessed { get; set; } = true;
        public string? ErrorMessage { get; set; } = string.Empty;
    }
}
