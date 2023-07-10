using MediatR;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;

namespace MRE.Application.Features.ProjectFeatures.Commands
{
    public class UpdateProjectCommand : IRequest<CqrsResponse>
    {
        public ProjectModel Model { get; set; }
        public UpdateProjectCommand(ProjectModel model)
        {
            Model = model;
        }

        public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, CqrsResponse>
        {
            private readonly IProjectRepository _projectRepository;

            public UpdateProjectCommandHandler(IProjectRepository projectRepository)
            {
                _projectRepository = projectRepository;
            }

            public async Task<CqrsResponse> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
            {
                var project = await Task.Run(() => _projectRepository.Update(command.Model));

                if (project is null)
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = "The recored you are trying to update does not exist"
                    };
                }

                return new UpdateProjectCommandResult
                {
                    Id = project.Id
                };
            }
        }

        public class UpdateProjectCommandResult : CqrsResponse
        {
            public Guid Id { get; set; }
        }
    }
}
