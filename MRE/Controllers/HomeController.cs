using MRE.Presistence.IProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MRE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return Ok("MRE - API Working");
        }
    }
}