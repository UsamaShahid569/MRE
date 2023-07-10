using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Models
{
    public class SuperAdminUserModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
