using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Contracts.Models
{
    public class StatusModel
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
    }
}
