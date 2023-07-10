using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class UpdateProfileCommand : IRequest<CqrsResponse>
    {
        public UserProfileModel Model { get; set; }
        public UpdateProfileCommand(UserProfileModel model)
        {
            Model = model;
        }

        public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, CqrsResponse>
        {
            private readonly IUserRepository _userRepository;

            public UpdateProfileCommandHandler(IUserRepository UserRepository)
            {
                _userRepository = UserRepository;
            }

            public async Task<CqrsResponse> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
            {

                await Task.Run(() => _userRepository.UpdateProfile(command.Model), cancellationToken);

                return new CqrsResponse()
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
        }


        
    }
}
