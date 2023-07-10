using MediatR;
using MRE.Application.Features.UserFeatures.Commands;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;

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
            public Task<CqrsResponse> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}
