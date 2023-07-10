using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MRE.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;

namespace MRE.Domain.Entities.Identity
{
    public class UserRole : DomainObject
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
