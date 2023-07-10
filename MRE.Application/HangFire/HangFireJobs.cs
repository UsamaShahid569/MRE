using Hangfire;
using MRE.Contracts.Models;
using MRE.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRE.Application.HangFire
{
    public class HangFireJobs
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public HangFireJobs(IMediator Mediator, IConfiguration Configuration)
        {
            _mediator = Mediator;
            _configuration = Configuration;
        }

        [AutomaticRetry(Attempts = 1)]
        public async Task Function()
        {
            if (bool.TryParse(_configuration["HangFire:DisableHangFire"], out bool result))
            {
                if (result != true)
                {
                     //await _mediator.Send(new AddCommandHere());
                }
            }
        }
        
    }
}
