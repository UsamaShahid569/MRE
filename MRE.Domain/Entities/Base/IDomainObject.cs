using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Domain.Entities.Base
{
    public interface IDomainObject
    {
        Guid Id { get; set; }
        DateTime? CreatedDate { get; set; }
        Guid? CreatedBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        Guid? UpdatedBy { get; set; }
        DateTime? ValidUntil { get; set; }
        public Guid? TenantId { get; set; }
        public bool? IsActive { get; set; }
    }
}
