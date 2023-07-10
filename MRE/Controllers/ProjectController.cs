using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRE.Application.Features.ProjectFeatures.Commands;
using MRE.Contracts.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace MRE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(GetProjectByIdCommand.GetProjectByIdCommandResult))]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new GetProjectByIdCommand(id)));
        } 
        
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(GetProjectListCommand.GetProjectListCommandResult))]
        public async Task<IActionResult> List()
        {
            return Ok(await _mediator.Send(new GetProjectListCommand()));
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CreateProjectCommand.CreateProjectCommandResult))]
        public async Task<IActionResult> Create([FromBody] ProjectModel model)
        {
            return Ok(await _mediator.Send(new CreateProjectCommand(model)));
        }


        [HttpPut]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UpdateProjectCommand.UpdateProjectCommandResult))]
        public async Task<IActionResult> Update([FromBody] ProjectModel model)
        {
            return Ok(await _mediator.Send(new UpdateProjectCommand(model)));
        }


        [HttpDelete("{id}")]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(void))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteProjectCommand(id)));
        }

    }
}
