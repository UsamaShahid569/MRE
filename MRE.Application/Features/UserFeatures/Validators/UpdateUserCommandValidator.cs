using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Application.Features.UserFeatures.Commands;

namespace MRE.Application.Features.UserFeatures.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty()
             .WithMessage("UserId cannot be empty");

            RuleFor(x => x.UserId).NotNull()
           .WithMessage("UserId cannot be null");

            RuleFor(x => x.UserId).NotEqual(Guid.Empty)
             .WithMessage("UserId cannot be empty");

           

            RuleFor(x => x.Model.Email).NotEmpty()
             .WithMessage("Email cannot be empty");

            RuleFor(x => x.Model.RoleId).NotEmpty()
            .WithMessage("Must have a role");
        }
    }
}
