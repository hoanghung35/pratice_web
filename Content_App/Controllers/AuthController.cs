using System.Security.Claims;
using Content_App.App.DTOs.Auth;
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

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                id = User.FindFirst("sub")?.Value,
                username = User.Identity?.Name,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            var result = await _authService.LoginAsync(dto);
            WriteCookies(result);
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (refreshToken == null) return Unauthorized();

            var result = await _authService.RefreshAsync(refreshToken);
            WriteCookies(result);
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refresh_token"];

            await _authService.LogoutAsync(refreshToken!);

            // clear cookies
            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return Ok();
        }


        private void WriteCookies(AuthResultDto result)
        {
            Response.Cookies.Append("access_token", result.AccessToken,
                new CookieOptions { 
                    HttpOnly = true, 
                    Secure = false, 
                    SameSite = SameSiteMode.Lax 
                });

            Response.Cookies.Append("refresh_token", result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
        }
    }
}
