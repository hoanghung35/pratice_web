using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Authorize(Policy = "DevOnly")]
    [ApiController]
    [Route("api/action")]
    public class ActionController : ControllerBase
    {
        [HttpDelete]
        public IActionResult DangerousAction()
        {
            return Ok("Dev only");
        }
    }
}
