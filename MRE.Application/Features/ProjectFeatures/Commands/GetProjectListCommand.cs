using MediatR;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Application.Features.ProjectFeatures.Commands
{
    public class GetProjectListCommand : IRequest<CqrsResponse>
    {
        public GetProjectListCommand()
        { }

        public class GetProjectListCommandHandler : IRequestHandler<GetProjectListCommand, CqrsResponse>
        {
            private readonly IProjectRepository _projectRepository;

            public GetProjectListCommandHandler(IProjectRepository projectRepository)
            {
                _projectRepository = projectRepository;
            }

            public async Task<CqrsResponse> Handle(GetProjectListCommand command, CancellationToken cancellationToken)
            {
                var projectDtos = await Task.Run(() => _projectRepository.List());

                if (projectDtos is null || !projectDtos.Any())
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = "No record found"
                    };
                }
                return new GetProjectListCommandResult
                {
                    ProjectDtos = projectDtos
                };
            }
        }

        public class GetProjectListCommandResult : CqrsResponse
        {
            public List<ProjectDto> ProjectDtos { get; set; }
        }
    }
}
