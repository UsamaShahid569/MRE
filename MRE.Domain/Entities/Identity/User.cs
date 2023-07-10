using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MRE.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MRE.Domain.Entities.Identity
{
    public class User : DomainObject
    {
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
