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

namespace MRE.Application.Features.LookupFeatures.Commands
{
    public class UpdateLookupCommand : IRequest<CqrsResponse>
    {
        
        public LookupModel Model { get; set; }
        public UpdateLookupCommand( LookupModel model)
        {
            
            Model = model;
        }

        public class UpdateLookupCommandHandler : IRequestHandler<UpdateLookupCommand, CqrsResponse>
        {
            private readonly ILookupRepository _lookupRepository;

            public UpdateLookupCommandHandler(ILookupRepository lookupRepository)
            {
                _lookupRepository = lookupRepository;
            }

            public async Task<CqrsResponse> Handle(UpdateLookupCommand command, CancellationToken cancellationToken)
            {
                


                await Task.Run(() => _lookupRepository.Update(command.Model));

                return new CqrsResponse
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    ErrorMessage = "Updated Successfully"
                };
            }
        }

       
    }
}
