using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Authorize(Policy = "ManagerUp")]
    [ApiController]
    [Route("api/approve")]
    public class ApproveController : ControllerBase
    {
        [HttpPost]
        public IActionResult Approve()
        {
            return Ok("Manager hoặc Dev");
        }
    }
}
