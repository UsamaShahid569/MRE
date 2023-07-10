using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MRE.Presistence.Abstruct;

namespace MRE.Application.Features.BusinessFeatures.Commands
{
    public class DeleteBusinessCommand : IRequest<Unit>
    {
        public Guid BussinessId { get; set; }

        public DeleteBusinessCommand(Guid bussinessId)
        {
            BussinessId = bussinessId;
        }

        public class DeleteBusinessCommandHandler : IRequestHandler<DeleteBusinessCommand, Unit>
        {
            private readonly IBusinessRepository _businessRepository;

            public DeleteBusinessCommandHandler(IBusinessRepository BusinessRepository)
            {
                _businessRepository = BusinessRepository;
            }

            public async Task<Unit> Handle(DeleteBusinessCommand command, CancellationToken cancellationToken)
            {
                await Task.Run(() => _businessRepository.Delete(command.BussinessId));

                return Unit.Value;
            }
        }
    }
}
