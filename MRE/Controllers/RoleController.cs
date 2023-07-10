using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MRE.Application.Features.RoleFeatures.Queries;
using MRE.Contracts.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MRE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator Mediator)
        {
            _mediator = Mediator;
        }


        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<RoleDto>))]
        public async Task<IActionResult> RolesQuery()
        {
            return Ok(await _mediator.Send(new RolesQuery()));
        }
    }
}