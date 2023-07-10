using MRE.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Domain.Entities.Business
{
    public class ContactPhone : DomainObject
    {
        public string PhoneNumber { get; set; }
        public bool IsPrimary { get; set; }
        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
