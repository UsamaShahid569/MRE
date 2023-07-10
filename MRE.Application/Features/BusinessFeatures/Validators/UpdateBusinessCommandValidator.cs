using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Application.Features.UserFeatures.Commands;
using MRE.Application.Features.BusinessFeatures.Commands;

namespace MRE.Application.Features.BusinessFeatures.Validators
{
    public class UpdateBusinessCommandValidator : AbstractValidator<UpdateBusinessCommand>
    {
        public UpdateBusinessCommandValidator()
        {
            RuleFor(x => x.Model.Id).NotEmpty()
             .WithMessage("BusinessId cannot be empty");

            RuleFor(x => x.Model.Id).NotNull()
           .WithMessage("BusinessId cannot be null");

        }
    }
}
