using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Presistence.Abstruct;

namespace MRE.Application.Features.UserFeatures.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
        {
            private readonly IUserRepository _userRepository;

            public DeleteUserCommandHandler(IUserRepository UserRepository)
            {
                _userRepository = UserRepository;
            }

            public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
            {
                await Task.Run(() => _userRepository.Delete(command.UserId));

                return Unit.Value;
            }
        }
    }
}
