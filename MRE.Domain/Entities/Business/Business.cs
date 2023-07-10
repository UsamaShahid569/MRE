using MRE.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Domain.Entities.Business
{
    public class Business : DomainObject
    {
        [Key]
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<BusinessGroup> BusinessGroups { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public bool Active { get; set; }
    }
}
