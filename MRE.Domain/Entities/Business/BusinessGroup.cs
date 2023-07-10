using MRE.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Domain.Entities.Business
{
    public class BusinessGroup : DomainObject
    {
        public Guid GroupId { get; set; }
        public virtual Group Group { get; set; }

        public Guid BusinessId { get; set; }
        public virtual Business Business { get; set; }


    }
}
