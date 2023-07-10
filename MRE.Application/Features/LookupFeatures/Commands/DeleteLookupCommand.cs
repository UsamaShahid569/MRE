using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Presistence.Abstruct;

namespace MRE.Application.Features.LookupFeatures.Commands
{
    public class DeleteLookupCommand : IRequest<CqrsResponse>
    {
        public Guid LookupId { get; set; }

        public DeleteLookupCommand(Guid lookupId)
        {
            LookupId = lookupId;
        }

        public class DeleteLookupCommandHandler : IRequestHandler<DeleteLookupCommand, CqrsResponse>
        {
            private readonly ILookupRepository _lookupRepository;

            public DeleteLookupCommandHandler(ILookupRepository lookupRepository)
            {
                _lookupRepository = lookupRepository;
            }

            public async Task<CqrsResponse> Handle(DeleteLookupCommand command, CancellationToken cancellationToken)
            {
                await Task.Run(() => _lookupRepository.Delete(command.LookupId));

                return new CqrsResponse
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    ErrorMessage = "Deleted Successfully"
                };
            }
        }
    }
}
