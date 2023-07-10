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

namespace MRE.Application.Features.UserFeatures.Queries
{
    public class UsersQuery : IRequest<DataAndCountDto<UserDto>>
    {
        public UsersQueryFilter Filter { get; set; }

        public UsersQuery(UsersQueryFilter filter)
        {
            Filter = filter;
        }

        public class UserQueryHandler : IRequestHandler<UsersQuery, DataAndCountDto<UserDto>>
        {
            private readonly IUserRepository _userRepository;

            public UserQueryHandler(IUserRepository UserRepository)
            {
                _userRepository = UserRepository;
            }

            public async Task<DataAndCountDto<UserDto>> Handle(UsersQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _userRepository.Get(request.Filter.Skip,request.Filter.Take,request.Filter.Search, request.Filter.OrderBy,request.Filter.RoleId));

                return result;
            }
        }
    }
}
