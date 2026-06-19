using Content_App.App.Services;
using Microsoft.AspNetCore.Mvc;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Content_App.Controllers
{
    [Route("api/request")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly RequestService _requestService;

       public RequestController(RequestService requestService) {
           this._requestService = requestService;
       }

       [Authorize]
       [HttpGet]
       public async Task<IActionResult> GetRequest() {
           var picId = new Guid(User.FindFirst(JwtClaimConstant.PicId)?.Value!);

           return Ok(await _requestService.GetRequestAsync(picId));
       }
    }
}
