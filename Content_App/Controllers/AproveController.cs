using Microsoft.AspNetCore.Mvc;
using Content_App.App.Service;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Content_App.Controllers
{
    [Route("api/approve-request")]
    [ApiController]
    public class AproveController : ControllerBase
    {
        private readonly ApproveService _approveService;

        public ApproveController(ApproveService service) {
            this._approveService = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetApprove() {
            var userId = User.FindFirst(JwtClaimConstant.Userid)?.Value!;
            var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;

            return Ok(await _approveSerive.GetApproveAsync(new Guid(userId), roleName));
        }

        [Authorize(Policy = "CanApproval")]
        [HttpPost("approve/{approveId}")]
        public async Task<IActionResult> ApproveRequest(Guid approveId) {
            var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;
            await _approveService.ApproveRequestAsync(approveId, roleName);

            return Ok();
        }

        [Authorize(Policy = "CanApproval")]
        [HttpPut("approve/all")]
        public async Task<IActionResult> ApproveAllRequest([FromBody] List<Guid> data) {
            var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;
            await _approveService.ApproveAllRequestAsync(data, roleName);

            return Ok();
        }

        [Authorize(Policy = "CanApproval")]
        [HttpPost("reject/{approveid}")]
        public async Task<IActionResult> RejectRequest(Guid approveId) {
            await _approveService.RejectRequestAsync(approveId);

            return Ok();
        }
    }
}
