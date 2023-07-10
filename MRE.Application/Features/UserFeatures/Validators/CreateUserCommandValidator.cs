using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Application.Features.UserFeatures.Commands;

namespace MRE.Application.Features.UserFeatures.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            
            RuleFor(x => x.Model.Email).NotEmpty()
                .WithMessage("Email cannot be empty");

            RuleFor(x => x.Model.FullName).NotEmpty()
               .WithMessage("Name cannot be empty");

            RuleFor(x => x.Model.RoleId).NotEmpty()
            .WithMessage("Must have a role");
        }
    }
}
