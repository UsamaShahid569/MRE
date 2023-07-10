using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using MRE.Presistence.IProvider;

namespace MRE.Application.Features.BusinessFeatures.Commands
{
    public class CreateBusinessCommand : IRequest<CqrsResponse>
    {
        public BusinessModel Model { get; set; }
        public CreateBusinessCommand(BusinessModel model)
        {
            Model = model;
        }

        public class CreateBusinessCommandHandler : IRequestHandler<CreateBusinessCommand, CqrsResponse>
        {
            private readonly IBusinessRepository _businessRepository;

            public CreateBusinessCommandHandler(IBusinessRepository BusinessRepository)
            {
                _businessRepository = BusinessRepository;
            }

            public async Task<CqrsResponse> Handle(CreateBusinessCommand command, CancellationToken cancellationToken)
            {

                var businessId = await Task.Run(() => _businessRepository.Create(command.Model));

                return new CreateBusinessCommandResult
                {
                    BusinessId = businessId.Id
                };
            }
        }

        public class CreateBusinessCommandResult : CqrsResponse
        {
            public Guid BusinessId { get; set; }
        }
    }
}
