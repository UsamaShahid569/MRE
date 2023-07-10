using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Contracts.Filters.Base
{
    public class BaseFilter
    {
        public string? Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string? OrderBy { get; set; }
    }
}
