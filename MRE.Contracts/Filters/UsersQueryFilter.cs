using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Contracts.Filters.Base;

namespace MRE.Contracts.Filters
{
    public class UsersQueryFilter : BaseFilter
    {
        public Guid? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? LastLoginBefore { get; set; }
    }
}
