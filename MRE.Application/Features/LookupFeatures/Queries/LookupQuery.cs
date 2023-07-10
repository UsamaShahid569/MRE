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

namespace MRE.Application.Features.LookupFeatures.Queries
{
    public class LookupQuery : IRequest<LookupDto>
    {
        public Guid LookupId { get; set; }

        public LookupQuery(Guid lookupId)
        {
            LookupId = lookupId;
        }

        public class LookupQueryHandler : IRequestHandler<LookupQuery, LookupDto>
        {
            private readonly ILookupRepository _lookupRepository;

            public LookupQueryHandler(ILookupRepository lookupRepository)
            {
                _lookupRepository = lookupRepository;
            }

            public async Task<LookupDto> Handle(LookupQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _lookupRepository.GetById(request.LookupId));

                return result;
            }
           
        }
    }
}
