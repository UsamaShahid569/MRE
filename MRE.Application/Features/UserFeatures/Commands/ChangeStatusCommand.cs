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
using MediatR;

namespace MRE.Application.Features.UserFeatures.Commands
{
    public class ChangeStatusCommand : IRequest<CqrsResponse>
    {
        public Guid Id { get; set; }
        public bool State { get; set; }
        public ChangeStatusCommand(Guid userId, bool isActive)
        {
            Id = userId;
            State = isActive;
        }

        public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand, CqrsResponse>
        {
            private readonly IUserRepository _userRepository;

            public ChangeStatusCommandHandler(IUserRepository UserRepository)
            {
                _userRepository = UserRepository;
            }

            public async Task<CqrsResponse> Handle(ChangeStatusCommand command, CancellationToken cancellationToken)
            {
                

                var status = await Task.Run(() => _userRepository.ChangeStatus(command.Id,command.State));

                return new ChangeStatusCommandResult
                {
                    Status = status
                };
            }
        }


        public class ChangeStatusCommandResult : CqrsResponse
        {
            public bool Status { get; set; }
        }
    }
}
