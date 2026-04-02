using Content_App.App.Services;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService) {
            this._orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            return Ok(await _orderService.GetOrderAsync());
        }
    }
}
