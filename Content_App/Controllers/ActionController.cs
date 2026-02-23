using Content_App.App.DTOs.Item;
using Content_App.App.DTOs.User;
using Content_App.App.Services;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Content_App.Controllers
{
    [Route("api/action")]
    [ApiController]
    public class ActionController : Controller
    {
        private readonly LogDbContext _context;
        private readonly ILogger<ActionController> _logger;
        private readonly FileService _fileService;

        public ActionController(LogDbContext context, ILogger<ActionController> logger, FileService fileService)
        {
            this._context = context;
            this._logger = logger;
            this._fileService = fileService;
        }

        [Authorize(Policy ="dev")]
        [HttpPost("create-item")]
        public async Task CreateItem(ItemDto dto)
        {
            try
            {
                var item = new Item
                {
                    VnName = dto.VnName,
                    EnName = dto.EnName,
                    Maker = dto.Maker,
                    Supplier = dto.Supplier,
                    PositionIn = dto.PositionIn,
                    Quantity = dto.Quantity,
                    Unit = dto.Unit,
                    Cost = dto.Cost,
                    Image = dto.Image
                };

                _context.Items.Add(item);
                await _context.SaveChangesAsync(); 
            }
            catch(Exception err)
            {
                _logger.LogError(err, "Error occur when create item");
            }
        }

        [HttpGet("invent")]
        public async Task<IActionResult> Inventory()
        {
            var userDto = new UserDto 
            { 
                DepId = User.FindFirst(JwtClaimConstant.UserId)?.Value,
                RoleName = User.FindFirst(JwtClaimConstant.Role)?.Value
            };

            var fileName = $"Item{DateTime.UtcNow.AddHours(7).ToString("yyyyMMdd")}.xlsx";
            byte[] fileData = await _fileService.ExportItemAsync(userDto);

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
