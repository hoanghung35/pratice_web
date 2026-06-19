using Content_App.App.DTOs.Item;
using Content_App.App.Services;
using Content_App.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace Content_App.Controllers
{
    [Route("api/items")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class ItemController : Controller
    {
        private readonly ItemService _itemService;
        private readonly ActionService _actionService;
        private readonly FileService _fileService;
        private readonly RequestService _requestService;
        private readonly ApproveService _approveService;

        public ItemController(ItemService itemService, 
            ActionService actionService, 
            FileService fileService, 
            RequestService requestService, 
            ApproveService approceService
            )
        {
            this._itemService = itemService;
            this._actionService = actionService;
            this._fileService = fileService;
            this._requestService = requestService;
            this._approveService = approveService;
        }

       [Authorize]
       [HttpGet]
       public async Task<IActionResult> GetItem() {
           var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;
           var userId = User.FindFirst(JwtClaimConstant.UserId)?.Value!;

           return Ok(await _itemService.GetItemAsync(roleName, new Guid(userId)));
       }

       [Authorize]
       [HttpPost("create-item")]
       public async Task<IActionResult> CreateItem([FromBody] ItemDto dto) {
           var userId = User.FindFirst(JwtClaimConstant.UserId)?.Value!;
           var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;
           var picId = User.FindFirst(JwtClaimConstant.PicId)?.Value!;
           var newItemId = await _itemService.CreateItemAsync(newItem, new Guid(userId));
           await _actionService.NewItemActionAsync(newItem, new Guid(userId), new Guid(picId), newItemId);

           return Ok();
       }

       [Authorize]
       [HttpPut("update-item")]
       public async Task<IActionResult> UpdateItem(ItemDto item) {
           await _itemService.UpdateItemAsync(item);

           return Ok();
       }

       [Authorize]
       [HttpPost("change-image/{itemId}")]
       public async Task<IActionResult> ItemUpdateImage(IFormFile file, Guid itemId) {
           await _itemService.ItemUpdateImageAsync(file, itemId);

           return Ok();
       }

       [Authorize]
       [HttpPut("delete-item/{id}")]
       public async Task<IActionResult> DeleteItem(Guid itemId) {
           var picId = User.FindFirst(JwtClaimConstant.PicId)?.Value!;
           await _itemService.DeleteItemAsync(itemId, new Guid(picId));

           return Ok();
       }

       [Authorize]
       [HttpGet("export-item")]
       public async Task<IActionResult> ExportFileItem() {
           var userId = User.FindFirst(JwtClaimConstant.UserId)?.Value!;
           var role = User.FindFirst(JwtClaimConstant.Role)?.Value!;
           var fileName = DateTime.UtcNow.AddHours(7).ToString("dd-MM-yyyy") + "item.xlsx";
           var data = await _fileService.ExportFileItem(new Guid(userId), role);

           return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
       }

       [Authorize]
       [HttpPut("receive-item")]
       public async Task<IActionResult> ReceiveItem([FromBody] ItemActivityDto dto) {
           var userId = new Guid(User.FindFirst(JwtClaimConstant.UserId)?.Value!);
           var picId = new Guid(User.FindFirst(JwtClaimConstant.PicId)?.Value!);
           await _actionService.ReceiveItemAsync(dto, userId, picid);

           return Ok();
       }

       [Authorize]
       [HttpPut("delivery-item")]
       public async Task<IActionResult> DeliveryItem([FromBody] ItemActivityDto dto) {
           var userId = new Guid(User.FindFirst(JwtClaimConstant.UserId)?.Value!);
           var picId = new Guid(User.FindFirst(JwtClaimConstant.PicId)?.Value!);

           var res = await _actionService.DeliveryItemAsync(dto, userId, picId);
           var reqId = await _requestService.CreateRequestAsync(res, picId);
           await _approveService.CreateApproveAsync(null, reqId);

           return Ok();
       }

       [Authorize]
       [HttpPost("create-list-item")]
       public async Task<IActionResult> CreateListItemFromFile(IFormFile file) {
           var userId = new Guid(User.FindFirst(JwtClaimConstant.UserId)?.Value!);
           var picId = new Guid(User.FindFirst(JwtClaimConstant.PicId)?.Value!);

           await _itemService.CreateListItemFromFileAsync(file, userId, picId);

           return Ok();
       }
    }
}
