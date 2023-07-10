using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Application.Features.UserFeatures.Queries;

namespace MRE.Application.Features.UserFeatures.Validators
{
   public class UserQueryValidator : AbstractValidator<UserQuery>
    {
        public UserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
           .WithMessage("UserId cannot be empty");

            RuleFor(x => x.UserId).NotEqual(Guid.Empty)
             .WithMessage("UserId cannot be empty");

            RuleFor(x => x.UserId).NotNull()
           .WithMessage("UserId cannot be null");
        }
    }
}
