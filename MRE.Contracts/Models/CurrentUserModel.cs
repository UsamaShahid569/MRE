using MRE.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Models
{
    public class CurrentUserModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public Guid? TenantId { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
