using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using MRE.Presistence.IProvider;
using MediatR;

namespace MRE.Application.Features.BusinessFeatures.Commands
{
    public class UpdateBusinessCommand : IRequest<CqrsResponse>
    {
        public BusinessModel Model { get; set; }
        public UpdateBusinessCommand(BusinessModel model)
        {
            Model = model;
        }

        public class UpdateBusinessCommandHandler : IRequestHandler<UpdateBusinessCommand, CqrsResponse>
        {
            private readonly IBusinessRepository _businessRepository;

            public UpdateBusinessCommandHandler(IBusinessRepository BusinessRepository)
            {
                _businessRepository = BusinessRepository;
            }

            public async Task<CqrsResponse> Handle(UpdateBusinessCommand command, CancellationToken cancellationToken)
            {

                var businessId = await Task.Run(() => _businessRepository.Update(command.Model), cancellationToken);

                return new UpdateBusinessCommandResult()
                {
                    BusinessId = businessId.Id
                };
            }
        }
        public class UpdateBusinessCommandResult : CqrsResponse
        {
            public Guid BusinessId { get; set; }
        }


    }
}
