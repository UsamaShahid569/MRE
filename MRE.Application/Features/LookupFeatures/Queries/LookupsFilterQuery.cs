using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Contracts.Filters;
using MRE.Presistence.Abstruct;
using MediatR;

namespace MRE.Application.Features.LookupFeatures.Queries
{
    public class LookupsFilterQuery : IRequest<DataAndCountDto<LookupDto>>
    {
        public LookupQueryFilter Filter { get; set; }

        public LookupsFilterQuery(LookupQueryFilter filter)
        {
            Filter = filter;
        }

        public class LookupsFilterQueryHandler : IRequestHandler<LookupsFilterQuery, DataAndCountDto<LookupDto>>
        {
            private readonly ILookupRepository _lookupRepository;

            public LookupsFilterQueryHandler(ILookupRepository LookupRepository)
            {
                _lookupRepository = LookupRepository;
            }

            public async Task<DataAndCountDto<LookupDto>> Handle(LookupsFilterQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _lookupRepository.GetAll(request.Filter));

                return result;
            }
        }
    }
}


