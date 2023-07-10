using MediatR;
using Microsoft.AspNetCore.Http;
using MRE.Application.Features.UserFeatures.Commands;
using MRE.Presistence.Abstruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Application.Features.ProjectFeatures.Commands
{
    public class DeleteProjectCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }

        public DeleteProjectCommand(Guid id)
        {
            Id = id;
        }

        public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit>
        {
            private readonly IProjectRepository _projectRepository;

            public DeleteProjectCommandHandler(IProjectRepository projectRepository)
            {
                _projectRepository = projectRepository;
            }

            public async Task<Unit> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
            {
                await Task.Run(() => _projectRepository.Delete(command.Id));

                return Unit.Value;
            }
        }
    }
}
