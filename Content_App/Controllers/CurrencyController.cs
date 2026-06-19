using Content_App.App.Services;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Route("api/currency")]
    [ApiController]
    public class ActionController : Controller
    {
        private readonly LogDbContext _context;
        private readonly CurrencyService _currencyService;

        public CurrencyController(CurrencyController service) {
            this._currencyService = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCurrency() {
            return Ok(await _currencyService.GetCurrencyAsync());
        }

        [Authorize]
        [HttpPost("create-new")]
        public async Task<IActionResult> CreatNewCurrency([FromBody] Currency currency) {
            await _currencyService.CreateNewCurrencyAsync(currency);

            return Ok();
        }

        [Authorize]
        [HttpPut("update-currency")]
        public async Task<IActionResult> UpdateCurrency([FromBody] Currency currency) {
            await _currencyService.UpdateCurrencyAsync(currency);
            return Ok();
        }

        [Authorize]
        [HttpDelete("delete-currency/{id}")]
        public async Task<IActionResult> DeleteCurrencyAsync(Guid id) {
            await _currencyService.DeleteCurrencyAsync(id);
            return Ok();
        }
    }
}
