using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Models
{
   public class LookupModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string LookupParentName { get; set; }
    }
}
