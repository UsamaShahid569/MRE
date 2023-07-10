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

namespace MRE.Application.Features.UserFeatures.Queries
{
    public class UserQuery : IRequest<CqrsResponse>
    {
        public Guid UserId { get; set; }

        public UserQuery(Guid userId)
        {
            UserId = userId;
        }

        public class UserQueryHandler : IRequestHandler<UserQuery, CqrsResponse>
        {
            private readonly IUserRepository _userRepository;

            public UserQueryHandler(IUserRepository UserRepository)
            {
                _userRepository = UserRepository;
            }

            public async Task<CqrsResponse> Handle(UserQuery request, CancellationToken cancellationToken)
            {
                var result = await Task.Run(() => _userRepository.Get(request.UserId));

                if (result == null)
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = "Not found"
                    };
                }
                return new UserQueryHandlerResult
                {
                    User = result
                };
            }
            public class UserQueryHandlerResult : CqrsResponse
            {
                public UserModel User { get; set; }
            }
        }
    }
}
