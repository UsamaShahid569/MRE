using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Dtos
{
    public class LookupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string LookupParentName { get; set; }
    }
}
