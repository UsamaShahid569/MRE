using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Contracts.Filters.Base;

namespace MRE.Contracts.Filters
{
    public class LookupQueryFilter : BaseFilter
    {
        public string LookupParentName { get; set; }
    }
}

