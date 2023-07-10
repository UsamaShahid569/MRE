using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Models
{
    public class UserModel
    {
        public Guid? Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Guid? RoleId { get; set; }
    }
}
