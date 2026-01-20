using Content_App.App.Services;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Authorize(Policy = "Admin, Dev")]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _service;

        public OrderController(OrderService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            return Ok(await _service.GetByUserAsync());
        }
    }
}
