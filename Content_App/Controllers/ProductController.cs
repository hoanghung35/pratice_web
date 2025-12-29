using Content_App.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/items")]
    public class ProductController:ControllerBase
    {
        private readonly ItemService _service;

        public ProductController(ItemService service)
        {
            _service = service;
        }

        [HttpGet("area/{areaId}")]
        public async Task<IActionResult> GetByArea(Guid areaId)
        {
            return Ok(await _service.GetByAreaAsync(areaId));
        }
    }
}
