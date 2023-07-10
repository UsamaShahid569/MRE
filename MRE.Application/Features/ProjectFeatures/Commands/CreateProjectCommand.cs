using MediatR;
using MRE.Application.Features.UserFeatures.Commands;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;

namespace MRE.Application.Features.ProjectFeatures.Commands
{
    public class CreateProjectCommand : IRequest<CqrsResponse>
    {
        public ProjectModel ProjectModel { get; set; }

        public CreateProjectCommand(ProjectModel projectModel)
        {
            ProjectModel = projectModel;
        }

        public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, CqrsResponse>
        {
            private readonly IProjectRepository _repository;

            public CreateProjectCommandHandler(IProjectRepository repository)
            {
                _repository = repository;
            }

            public async Task<CqrsResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
            {
                var project = await Task.Run(() => _repository.Create(request.ProjectModel));
                if(project == null)
                {
                    return new CqrsResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ErrorMessage = "Record not created"
                    };
                }

                return new CreateProjectCommandResult
                {
                    Id = project.Id
                };
            }
        }

        public class CreateProjectCommandResult : CqrsResponse
        {
            public Guid Id { get; set; }
        }
    }
}
