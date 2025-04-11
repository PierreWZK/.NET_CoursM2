using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize]
    public class AdminController : ControllerBase
    {
        [HttpGet("crash")]
        [Authorize(Roles = "admin")]
        public IActionResult Crash()
        {
            return Ok("Bouuummm");
        }

        [HttpGet("message")]
        [Authorize(Roles = "admin")]
        public IActionResult Message()
        {
            throw new AppException("SimulatedCrash");
        }
    }

}
