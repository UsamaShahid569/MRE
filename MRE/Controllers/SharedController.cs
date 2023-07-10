using MRE.Application.Features.SharedFeatures.Queries;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security.Policy;
using MRE.Contracts.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using MRE.Application.Features.AuthFeatures.Commands;

namespace MRE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedController : Controller
    {
        private readonly IMediator _mediator;

        public SharedController(IMediator Mediator)
        {
            _mediator = Mediator;
        }
        [HttpPut("status")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CqrsResponse))]
        public async Task<IActionResult> UpdateTableStatus([FromBody] StatusModel model)
        {
            return Ok(await _mediator.Send(new UpdateStatusCommand(model)));
        }
        [HttpGet("dropdown/{entity}/{id?}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DropdownDto))]
        public async Task<dynamic> DropdownQuery([FromRoute] string entity,Guid? id = null)
        {
            return Ok(await _mediator.Send(new DropdownQuery(entity,id)));
        }


    }
}
