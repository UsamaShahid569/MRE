using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Dtos
{
    public class AuthUserDto : CqrsResponse
    {
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
        public TokenUserInfoDto UserInfo { get; set; }
    }
}
