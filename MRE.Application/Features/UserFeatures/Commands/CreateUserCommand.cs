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
using MRE.Presistence.IProvider;

namespace MRE.Application.Features.UserFeatures.Commands
{
    public class CreateUserCommand : IRequest<CqrsResponse>
    {
        public UserModel Model { get; set; }
        public CreateUserCommand(UserModel model)
        {
            Model = model;
        }

        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CqrsResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly ICurrentUserProvider _currentUserProvider;
            private readonly IRoleRepository _roleRepository;

            public CreateUserCommandHandler(IUserRepository UserRepository, ICurrentUserProvider CurrentUserProvider,
                 IRoleRepository RoleRepository)
            {
                _userRepository = UserRepository;
                _currentUserProvider = CurrentUserProvider;
                _roleRepository = RoleRepository;
            }

            public async Task<CqrsResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
            {
                var emailIsExist = _userRepository.Query().Any(x => x.Email == command.Model.Email);
                if (emailIsExist)
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = "Email already Exists"
                    };
                }

                

                var userId = await Task.Run(() => _userRepository.Create(command.Model));

                return new CreateUserCommandResult
                {
                    UserId = userId.Id
                };
            }
        }


        public class CreateUserCommandResult : CqrsResponse
        {
            public Guid UserId { get; set; }
        }
    }
}
