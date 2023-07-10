using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Domain.Entities.Identity;
using MRE.Presistence.Abstruct;
using MRE.Presistence.IProvider;
using MediatR;

namespace MRE.Application.Features.UserFeatures.Queries
{
    public class GetProfileDataQuery : IRequest<CqrsResponse>
    {

        public GetProfileDataQuery()
        {
        }

        public class GetProfileDataQueryHandler : IRequestHandler<GetProfileDataQuery, CqrsResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly ICurrentUserProvider _currentUser;

            public GetProfileDataQueryHandler(IUserRepository UserRepository, ICurrentUserProvider currentUser)
            {
                _userRepository = UserRepository;
                _currentUser = currentUser;
            }

            public async Task<CqrsResponse> Handle(GetProfileDataQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _userRepository.GetMemberProfile(), cancellationToken);

                if (result == null)
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = "Not found"
                    };
                }
                return new GetProfileDataResult
                {
                    User = result
                };
            }
            public class GetProfileDataResult : CqrsResponse
            {
                public UserProfileDto User { get; set; }
            }
        }
    }
}
