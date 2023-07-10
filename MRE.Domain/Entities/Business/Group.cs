using MRE.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Domain.Entities.Business
{
    public class Group : DomainObject
    {
        public string GroupName { get; set; }
        public virtual ICollection<BusinessGroup> BusinessGroups { get; set; }
    }
}
