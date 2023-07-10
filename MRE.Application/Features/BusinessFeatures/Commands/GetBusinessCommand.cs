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
    public class GetBusinessCommand : IRequest<CqrsResponse>
    {
        
        public class GetBusinessCommandHandler : IRequestHandler<GetBusinessCommand, CqrsResponse>
        {
            private readonly IBusinessRepository _businessRepository;

            public GetBusinessCommandHandler(IBusinessRepository BusinessRepository)
            {
                _businessRepository = BusinessRepository;
            }

            public async Task<CqrsResponse> Handle(GetBusinessCommand command, CancellationToken cancellationToken)
            {

                var business = await Task.Run(() => _businessRepository.GetAllBussiness(), cancellationToken);

                return new GetBusinessCommandResult()
                {
                    BusinessList = business ?? new List<Domain.Entities.Business.Business>()
                };
            }
        }
        public class GetBusinessCommandResult : CqrsResponse
        {
            public List<Domain.Entities.Business.Business> BusinessList { get; set; }
        }


    }
}
