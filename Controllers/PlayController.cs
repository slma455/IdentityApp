using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webApplication.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PlayController : ControllerBase
    {


        public PlayController() { }
        [Authorize]
        [HttpGet("Get-Players")]
        public IActionResult GetPlayers()
        {
            return Ok(new JsonResult( new { message = "Only Authorized users" }));
        }
    }
}
