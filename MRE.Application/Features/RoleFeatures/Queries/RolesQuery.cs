using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Presistence.Abstruct;
using MediatR;

namespace MRE.Application.Features.RoleFeatures.Queries
{
    public class RolesQuery : IRequest<List<RoleDto>>
    {

        public RolesQuery()
        {
        }

        public class RolesQueryHandler : IRequestHandler<RolesQuery, List<RoleDto>>
        {
            private readonly IRoleRepository _roleRepository;

            public RolesQueryHandler(IRoleRepository roleRepository)
            {
                _roleRepository = roleRepository;
            }

            public async Task<List<RoleDto>> Handle(RolesQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _roleRepository.Get());

                return result;
            }
        }
    }
}
