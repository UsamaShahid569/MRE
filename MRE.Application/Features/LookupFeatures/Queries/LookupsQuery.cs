using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Contracts.Filters;
using MRE.Presistence.Abstruct;

namespace MRE.Application.Features.LookupFeatures.Queries
{
    public class LookupsQuery : IRequest<List<LookupDto>>
    {
        public string LookupParent { get; set; }
        public LookupsQuery(string lookupParent)
        {
            LookupParent = lookupParent;
        }

        public class LookupsQueryHandler : IRequestHandler<LookupsQuery, List<LookupDto>>
        {
            private readonly ILookupRepository _lookupRepository;

            public LookupsQueryHandler(ILookupRepository lookupRepository)
            {
                _lookupRepository = lookupRepository;
            }

            public async Task<List<LookupDto>> Handle(LookupsQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _lookupRepository.Get(request.LookupParent));

                return result;
            }
        }
    }
}
