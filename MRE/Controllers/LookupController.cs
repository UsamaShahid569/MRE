using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MRE.Application.Features.LookupFeatures.Commands;
using MRE.Application.Features.LookupFeatures.Queries;
using MRE.Application.Features.UserFeatures.Queries;
using MRE.Contracts.Dtos;
using MRE.Contracts.Filters;
using MRE.Contracts.Models;
using MRE.Presistence.Abstruct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using Swashbuckle.AspNetCore.Annotations;


namespace MRE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : Controller
    {
        
        private readonly IMediator _mediator;

        public LookupController(IMediator Mediator)
        {
            _mediator = Mediator;
        }
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DataAndCountDto<LookupDto>))]
        public async Task<IActionResult> LookupsQueryPaging([FromQuery] LookupQueryFilter filter)
        {
            return Ok(await _mediator.Send(new LookupsFilterQuery(filter)));
        }

        [HttpGet("{lookupParentName}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<LookupDto>))]
        public async Task<IActionResult> LookupsQuery(string lookupParentName)
        {
            return Ok(await _mediator.Send(new LookupsQuery(lookupParentName)));
        }

        [HttpGet("get/{lookupId}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(LookupDto))]
        public async Task<IActionResult> LookupQuery(Guid lookupId)
        {
            return Ok(await _mediator.Send(new LookupQuery(lookupId)));
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CreateLookupCommand.CreateLookupResult))]
        public async Task<IActionResult> CreateLookup([FromBody] LookupModel model)
        {
            return Ok(await _mediator.Send(new CreateLookupCommand(model)));
        }
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CqrsResponse))]
        public async Task<IActionResult> UpdateLookup([FromBody] LookupModel model)
        {
            return Ok(await _mediator.Send(new UpdateLookupCommand(model)));
        }


        [HttpDelete("{lookupId}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CqrsResponse))]
        public async Task<IActionResult> Delete([FromRoute] Guid lookupId)
        {
            return Ok(await _mediator.Send(new DeleteLookupCommand(lookupId)));
        }

        

    }
}