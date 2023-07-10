using System;
using System.Collections.Generic;
using System.Text;
using MRE.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MRE.Domain.Entities.Identity
{
    public class Role : DomainObject
    {
        public string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }

        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }
    }
}
