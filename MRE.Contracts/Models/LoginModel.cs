using MRE.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MRE.Contracts.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string TokenId { get; set; }
        public LoginTypeEnum Type { get; set; } = LoginTypeEnum.Microsoft;
    }
}
