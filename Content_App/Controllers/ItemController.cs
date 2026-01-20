using Content_App.App.Services;
using Content_App.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _service;

        public ItemController(ItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("area/{areaId}")]
        public async Task<IActionResult> GetByArea(Guid areaId)
        {
            return Ok(await _service.GetByAreaAsync(areaId));
        }

        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPost]
        public async Task<IActionResult> Create(Item dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
    }
}
