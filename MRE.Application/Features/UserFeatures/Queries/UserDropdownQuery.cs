using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MRE.Contracts.Dtos;
using MRE.Presistence.Abstruct;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MRE.Application.Features.UserFeatures.Queries
{
    public class UserDropdownQuery : IRequest<List<DropdownDto>>
    {
        public UserDropdownQuery()
        {
        }

        public class UserDropdownQueryHandler : IRequestHandler<UserDropdownQuery, List<DropdownDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;

            public UserDropdownQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<List<DropdownDto>> Handle(UserDropdownQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _userRepository.Query().ProjectTo<DropdownDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken));

                return result;
            }
        }
    }
}