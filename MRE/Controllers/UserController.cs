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

namespace MRE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator Mediator)
        {
            _mediator = Mediator;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DataAndCountDto<UserDto>))]
        public async Task<IActionResult> UsersQuery([FromQuery] UsersQueryFilter filter)
        {
            return Ok(await _mediator.Send(new UsersQuery(filter)));
        }

        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UserDto))]
        public async Task<dynamic> UserQuery([FromRoute] Guid id)
        {
            if (id == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                return null;
            }
            return Ok(await _mediator.Send(new UserQuery(id)));
        }

        [HttpGet("dropdown")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<DropdownDto>))]
        public async Task<IActionResult> DropdownQuery()
        {
            return Ok(await _mediator.Send(new UserDropdownQuery()));
        }

        

        [HttpGet("changeStatus/{userId}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(ChangeStatusCommand.ChangeStatusCommandResult))]
        public async Task<dynamic> ChangeStatus(Guid userId, bool isActive)
        {
            return Ok(await _mediator.Send(new ChangeStatusCommand(userId,isActive)));
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CreateUserCommand.CreateUserCommandResult))]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            return Ok(await _mediator.Send(new CreateUserCommand(model)));
        }
        

        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(UpdateUserCommand.UpdateUserCommandResult))]
        public async Task<IActionResult> UpdateUser( [FromBody] UserModel model)
        {
            return Ok(await _mediator.Send(new UpdateUserCommand(model.Id.Value, model)));
        }


        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(void))]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            return Ok(await _mediator.Send(new DeleteUserCommand(id)));
        }

        
    }
}