using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MRE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;

        [HttpPost]
        public IActionResult Create()
        {
            return Ok();
        }
    }
}
