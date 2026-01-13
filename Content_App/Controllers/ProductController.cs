using Content_App.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateItem()
        {
            return Ok("Allow dev, manage, admin");
        }
    }
}
