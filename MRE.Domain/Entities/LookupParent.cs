using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MRE.Domain.Entities
{
        public class LookupParent
        {
            [Key]
            public Guid Id { get; set; }
            public string Name { get; set; }

            public virtual ICollection<Lookup> Lookups { get; set; }
        }
    }
