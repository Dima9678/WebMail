using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models.Requests
{
    public class ForwardRequest
    {
        public string ForwardEmail { get; set; }
        public Guid LetterId { get; set; }
    }
}
