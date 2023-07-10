using AutoMapper;
using AutoMapper.QueryableExtensions;
using MRE.Contracts.Dtos;
using MRE.Presistence.Abstruct;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Application.Features.SharedFeatures.Queries
{
    public class DropdownQuery : IRequest<List<DropdownDto>>
    {
        string Entity { get; }
        Guid? Id { get; }
        public DropdownQuery(string entity, Guid? id)
        {
            Entity = entity;
            Id = id;
        }

        public class DropdownQueryHandler : IRequestHandler<DropdownQuery, List<DropdownDto>>
        {
            private readonly IUserRepository _userRepository;
            
            private readonly IMapper _mapper;

            public DropdownQueryHandler(IUserRepository userRepository,
                IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

            public async Task<List<DropdownDto>> Handle(DropdownQuery request, CancellationToken cancellationToken)
            {
                var result = new List<DropdownDto>();
                switch (request.Entity.ToLower())
                {
                    case "user":
                        result = await Task.Run(() => _userRepository.Query()
                        .ProjectTo<DropdownDto>(_mapper.ConfigurationProvider)
                        .ToListAsync(cancellationToken: cancellationToken));
                        break;

                }
                return result;
            }
        }
    }
}