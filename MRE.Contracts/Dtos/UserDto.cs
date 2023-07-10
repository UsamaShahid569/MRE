using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MRE.Contracts.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        [IgnoreDataMember]
        public Guid? TenantId { get; set; }
    }
}
