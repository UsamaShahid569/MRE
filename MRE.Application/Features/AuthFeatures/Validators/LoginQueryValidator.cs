using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Application.Features.AuthFeatures.Queries;

namespace MRE.Application.Features.AuthFeatures.Validators
{
    public class LoginQueryValidator : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidator()
        {
            RuleFor(x=>x.Model.Email).NotEmpty()
                .WithMessage("Email cannot be empty");
            RuleFor(x => x.Model.TokenId).NotEmpty()
              .WithMessage("TokenId cannot be empty");
        }
    }
}
