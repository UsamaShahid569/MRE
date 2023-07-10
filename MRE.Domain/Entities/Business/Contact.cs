using MRE.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Domain.Entities.Business
{
    public class Contact : DomainObject
    {
        public int ContactType { get; set; }
        public string Name { get; set; }
        public string LegalName { get; set; }
        public virtual ICollection<ContactPhone> PhoneNumbers { get; set; }
        public virtual ICollection<ContactEmail> Emails { get; set; }
        public Guid BusinessId { get; set; }
        public virtual Business Business { get; set; }
    }
}
