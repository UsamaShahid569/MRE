using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Application.Features.BusinessFeatures.Commands;

namespace MRE.Application.Features.BusinessFeatures.Validators
{
    public class CreateBusinessCommandValidator : AbstractValidator<DeleteBusinessCommand>
    {
        public CreateBusinessCommandValidator()
        {
            RuleFor(x => x.BussinessId).NotEmpty()
             .WithMessage("BussinessId cannot be empty");

            RuleFor(x => x.BussinessId).NotEqual(Guid.Empty)
             .WithMessage("BussinessId cannot be empty");
        }
    }
}
