using MRE.Domain.Entities.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Contracts.Models
{
    public class BusinessModel
    {
        public Guid Id { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<BusinessGroup> BusinessGroups { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
    }
}
