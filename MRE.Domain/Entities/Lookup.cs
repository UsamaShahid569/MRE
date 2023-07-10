using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MRE.Domain.Entities
{
    public class Lookup
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public Guid LookupParentId { get; set; }

        [ForeignKey(nameof(LookupParentId))]
        public virtual LookupParent LookupParent { get; set; }
    }
}
