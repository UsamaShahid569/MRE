using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Application.Features.AuthFeatures.Commands
{
    public class UpdateStatusCommand : IRequest<CqrsResponse>
    {
        public StatusModel Model { get; set; }
        public UpdateStatusCommand(StatusModel model)
        {
            Model = model;
        }

        public class UpdateStatusCommandHandler : IRequestHandler<UpdateStatusCommand, CqrsResponse>
        {
            private readonly IUserRepository _userRepository;

            public UpdateStatusCommandHandler(
                IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<CqrsResponse> Handle(UpdateStatusCommand command, CancellationToken cancellationToken)
            {
                switch (command.Model.Type)
                {
                    
                    case "user":
                        await Task.Run(() => _userRepository.UpdateStatus(command.Model.Id, command.Model.Status));
                        break;
                    
                }
                

                return new CqrsResponse();
            }
        }


        
    }
}

