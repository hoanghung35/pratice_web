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
    [Route("api")]
    [ApiController]
    public class ActionController : Controller
    {
        private readonly LogDbContext _context;
        private readonly FileService _fileService;
        private readonly ILogger<Actioncontroller> _logger;
        private readonly ActionService _actionService;
        
       public ActionController(LogDbContext context, ILogger<ActionController> logger, FileService fileService, ActionService actionService) {
            this._context = context;
            this._logger = logger;
            this._fileService = fileService;
            this._actionService = actionService;
       }

       [Authorize]
       [HttpGet("invent")]
       public async Task<IActionResult> GetInvent() {
            var userId = User.FindFirst(JwtClainConstant.UserId)?.Value!;
            var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;

            DateTime  now = DateTime.UtcNow.AddHours(7);
            var fromD = new DateTime(now.Year, now.Month, 1, 0, 0, 0, 0);
            var toD = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59, 999);

            return Ok(await _actionService.GetInventAsync(new Guid(userId), roleName, fromD, toD))
       }

       [Authorize]
       [HttpGet("invent/condition")]
       public async Task<IActionResult> GetInventWithCondition([FromQuery] InventConditionDto dto) {
            var userId = User.FindFirst(JwtClainConstant.UserId)?.Value!;
            var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;

            //Convert string to Date
            dto.fromD = new DateTime(dto.fromD.Year, dto.fromD.Month, dto.fromD.Day, 0, 0, 0, 0);
            dto.toD = new DateTime(dto.toD.Year, dto.toD.Month, dto.toD.Day, 23, 59, 59, 999);

            return Ok(await _actionService.GetInventAsync(new Guid(userId), roleName, dto.fromD, dto.toD))
       }

       [Authorize]
       [HttpPost("invent/export")]
       public async Task<IActionResult> ExportFileInvent([FromBody] List<InventDto> dto) {
           var fileName = "Report.xlsx";
           var data = await _fileService.ExportFileInventAsync(dto);

           return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
       }

       [Authorize]
       [HttpGet("history")]
       public async Task<IActionResult> GetHistory() {
           var userId = User.FindFirst(JwtClaimConstant.UserId)?.Value!;
           var roleName = User.FindFirst(JwtClaimConstant.Role)?.Value!;

           return Ok(await _actionService.GetHistoryAsync(new Guid(userId), roleName));
       }

       [Authorize]
       [HttpPost("history/export")]
       public async Task<IActionResult> ExportFileHistory([FromBody] List<HistoryDto> dto) {
           var fileName = "history.xlsx";
           var data = await _fileService.ExportFileHistoryAsync(dto);

           return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
       }
    }
}
