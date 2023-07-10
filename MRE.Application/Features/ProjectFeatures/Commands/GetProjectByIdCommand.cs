using MediatR;
using Microsoft.AspNetCore.Http;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MRE.Application.Features.ProjectFeatures.Commands.GetProjectByIdCommand;

namespace MRE.Application.Features.ProjectFeatures.Commands
{
    public class GetProjectByIdCommand : IRequest<CqrsResponse>
    {
        public Guid Id { get; set; }
        public ProjectModel Model { get; set; }
        public GetProjectByIdCommand(Guid id)
        {
            Id = id;
        }

        public class GetProjectByIdCmmandHandler : IRequestHandler<GetProjectByIdCommand, CqrsResponse>
        {
            private readonly IProjectRepository _projectRepository;

            public GetProjectByIdCmmandHandler(IProjectRepository projectRepository)
            {
                _projectRepository = projectRepository;
            }

            public async Task<CqrsResponse> Handle(GetProjectByIdCommand command, CancellationToken cancellationToken)
            {
                var project = await Task.Run(() => _projectRepository.GetById(command.Id));

                if (project is null)
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = $"The recored {command.Id} does not exist"
                    };
                }

                return new GetProjectByIdCommandResult
                {
                    ProjectDto = project
                };
            }
        }

        public class GetProjectByIdCommandResult : CqrsResponse
        {
            public ProjectDto ProjectDto { get; set; }
        }
    }
}
