using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Contracts.Dtos
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
