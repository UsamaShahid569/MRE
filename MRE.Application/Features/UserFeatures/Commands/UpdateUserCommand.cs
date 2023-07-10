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

namespace MRE.Application.Features.UserFeatures.Commands
{
    public class UpdateUserCommand : IRequest<CqrsResponse>
    {
        public Guid UserId { get; set; }
        public UserModel Model { get; set; }
        public UpdateUserCommand(Guid userId, UserModel model)
        {
            UserId = userId;
            Model = model;
        }

        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CqrsResponse>
        {
            private readonly IUserRepository _userRepository;

            public UpdateUserCommandHandler(IUserRepository UserRepository)
            {
                _userRepository = UserRepository;
            }

            public async Task<CqrsResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                var emailIsExist = _userRepository.Query().Any(x => x.Id != command.UserId && x.Email == command.Model.Email);
                if (emailIsExist)
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = "Email already exists"
                    };
                }


                var userId = await Task.Run(() => _userRepository.Update(command.Model));

                return new UpdateUserCommandResult
                {
                    UserId = userId.Id
                };
            }
        }

        public class UpdateUserCommandResult : CqrsResponse
        {
            public Guid UserId { get; set; }
        }
    }
}
