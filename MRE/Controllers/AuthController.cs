using System;
using System.Net;
using System.Threading.Tasks;
using MRE.Application.Features.AuthFeatures.Queries;
using MRE.Contracts.Dtos;
using MRE.Contracts.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MRE.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        public AuthController( IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpPost("Login")]
        [AllowAnonymous]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(CqrsResponse))]
        public async Task<dynamic> Login([FromBody] LoginModel model)
        {
            return Ok(await _mediator.Send(new LoginQuery(model)));
        }

    }
}