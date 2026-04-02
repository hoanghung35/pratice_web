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
    public class ItemController : Controller
    {
        private readonly ItemService _itemService;
        private readonly ActionService _actionService;

        public ItemController(ItemService itemService, ActionService actionService)
        {
            this._itemService = itemService;
            this._actionService = actionService;
        }

        [Authorize(Policy ="dev")]
        [HttpGet]
        public async Task<IActionResult> GetAllItem()
        {
            return Ok(await _itemService.GetAllAsync());
        }

        [Authorize(Policy ="admin")]
        [HttpGet("limit")]
        public async Task<IActionResult> GetItemByLocation(Guid deptId, Guid areaId)
        {
            return Ok(await _itemService.GetItemByLocationAsync(deptId, areaId));
        }

        [Authorize(Policy = "CanCreate")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(ItemDto dto)
        {
            await _itemService.CreateAsync(dto);
            await _actionService.NewItemActionAsync(dto);

            return Ok();
        }

        [Authorize(Policy ="dev")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(Item item)
        {
            await _itemService.UpdateItemAsync(item);

            return Ok();
        }

        [Authorize(Policy ="dev")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            await _itemService.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("export-item")]
        public IActionResult Export()
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");
                // Đổ dữ liệu vào...
                var content = package.GetAsByteArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
            }
        }
    }
}
