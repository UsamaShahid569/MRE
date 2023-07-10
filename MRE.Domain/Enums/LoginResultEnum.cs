using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Domain.Enums
{
    public enum LoginResult
    {
        [Display(Name = "Successful Login")]
        Success,
        [Display(Name = "User Locked Out")]
        LockedOut,
        [Display(Name = "Verification Required")]
        RequiresVerification,
        [Display(Name = "Error While Logging in")]
        Failure
    }
}
