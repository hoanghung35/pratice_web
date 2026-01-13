using Content_App.App.Interfaces;
using Content_App.App.Interfaces.Authentication;
using Content_App.App.Services;
using Content_App.Domain.Entities;
using Content_App.Domain.Enums;
using Content_App.Infrastructure.Data;
using Content_App.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Content_App.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IRefreshTokenStore _refreshStore;
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(AuthService authService, IRefreshTokenStore refreshStore, AppDbContext context, IJwtService jwtService)
        {
            _authService = authService;
            _refreshStore = refreshStore;
            _context = context;
            _jwtService = jwtService;
        }

        //set cookie
        private void SetCookie(string name, string value, int minutes)
        {
            Response.Cookies.Append(name, value, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(minutes)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login()
        {
            // Demo account
            var account = new Account
            {
                Id = Guid.NewGuid(),
                Username = "dev01",
                Role = RoleCode.dev
            };

            var (access, refresh) =
                await _authService.GenerateTokenPairAsync(account);

            SetCookie("access_token", access, 30);
            SetCookie("refresh_token", refresh, 14 * 24 * 60);

            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (refreshToken == null) return Unauthorized();

            var hash = TokenHelper.Hash(refreshToken);
            var stored = await _refreshStore.FindAsync(hash);

            if (stored == null || stored.IsExpired || stored.IsRevoked)
                return Unauthorized();

            await _refreshStore.RevokeAsync(stored);

            var account = await _context.Accounts.FindAsync(stored.AccountId);
            if (account == null) return Unauthorized();

            var accessToken = _jwtService.GenerateAccessToken(account);

            var newRefresh = TokenHelper.GenerateRefreshToken();
            await _refreshStore.SaveAsync(new RefreshToken
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                TokenHash = TokenHelper.Hash(newRefresh),
                ExpiresAt = DateTime.UtcNow.AddDays(14)
            });

            SetCookie("access_token", accessToken, 30);
            SetCookie("refresh_token", newRefresh, 14 * 24 * 60);

            return Ok();
        }


        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (refreshToken != null)
            {
                var hash = TokenHelper.Hash(refreshToken);
                var stored = await _refreshStore.FindAsync(hash);
                if (stored != null)
                    await _refreshStore.RevokeAsync(stored);
            }

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return Ok();
        }

    }

}
