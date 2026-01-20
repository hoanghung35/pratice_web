using Content_App.App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Authorize(Policy = "Manager, Dev")]
    [ApiController]
    [Route("api/approve")]
    public class ApproveController : ControllerBase
    {
        private readonly ApproveService _service;

        public ApproveController(ApproveService service)
        {
            _service = service;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            return Ok(await _service.GetPendingAsync());
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(Guid id)
        {
            await _service.ApproveAsync(id);
            return Ok();
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(Guid id, [FromBody] string reason)
        {
            await _service.RejectAsync(id, reason);
            return Ok();
        }
    }
}
