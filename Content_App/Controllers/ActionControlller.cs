using Content_App.App.DTOs;
using Content_App.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Authorize(Policy = "Admin, Dev")]
    [ApiController]
    [Route("api/actions")]
    public class ActionController : ControllerBase
    {
        private readonly ActionService _service;

        public ActionController(ActionService service)
        {
            _service = service;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> Borrow(BorrowDto dto)
        {
            await _service.BorrowAsync(dto);
            return Ok();
        }

        [HttpPost("return")]
        public async Task<IActionResult> Return(ReturnDto dto)
        {
            await _service.ReturnAsync(dto);
            return Ok();
        }
    }
}
