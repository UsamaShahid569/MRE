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

namespace MRE.Application.Features.LookupFeatures.Commands
{
    public class CreateLookupCommand : IRequest<CqrsResponse>
    {
        public LookupModel Model { get; set; }
        public CreateLookupCommand(LookupModel model)
        {
            Model = model;
        }

        public class CreateLookupCommandHandler : IRequestHandler<CreateLookupCommand, CqrsResponse>
        {
            private readonly ILookupRepository _lookupRepository;

            public CreateLookupCommandHandler(ILookupRepository lookupRepository)
            {
                _lookupRepository = lookupRepository;
            }

            public async Task<CqrsResponse> Handle(CreateLookupCommand command, CancellationToken cancellationToken)
            {

                var result = await Task.Run(() => _lookupRepository.Add(command.Model));

                return new CreateLookupResult
                {
                    Id = result
                };
            }
        }


        public class CreateLookupResult : CqrsResponse
        {
            public Guid Id { get; set; }
        }
    }
}
