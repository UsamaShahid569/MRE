using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MRE.Application.Features.AuthFeatures.Queries;
using MRE.Application.Features.UserFeatures.Commands;
using MRE.Application.Features.UserFeatures.Queries;
using MRE.Contracts.Dtos;
using MRE.Contracts.Filters;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using MRE.Presistence.IProvider;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Swashbuckle.AspNetCore.Annotations;
using MRE.Application.Features.BusinessFeatures.Commands;

namespace MRE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : Controller
    {
        private readonly IMediator _mediator;

        public BusinessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(GetBusinessCommand.GetBusinessCommandResult))]
        public async Task<IActionResult> GetBusiness()
        {
            return Ok(await _mediator.Send(new GetBusinessCommand()));
        }


        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CreateBusinessCommand.CreateBusinessCommandResult))]
        public async Task<IActionResult> CreateBusiness([FromBody] BusinessModel model)
        {
            return Ok(await _mediator.Send(new CreateBusinessCommand(model)));
        }
        

        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UpdateBusinessCommand.UpdateBusinessCommandResult))]
        public async Task<IActionResult> UpdateBusiness( [FromBody] BusinessModel model)
        {
            return Ok(await _mediator.Send(new UpdateBusinessCommand(model)));
        }


        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(void))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteBusinessCommand(id)));
        }

        
    }
}