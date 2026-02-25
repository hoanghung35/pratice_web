using System.Security.Claims;
using Content_App.App.DTOs.Auth;
using Content_App.App.Services;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content_App.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet("infor")]
        public IActionResult GetInfor()
        {
            return Ok(new
            {
                userid = User.FindFirst("userid")?.Value,
                username = User.FindFirst("fullname")?.Value,
                role = User.FindFirst("rolename")?.Value,
                roleid = User.FindFirst("roleid")?.Value
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var res = await _authService.LoginAsync(dto);

            CookieInit(res);

            return Ok(new { message  = "Login Successfully!" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh_token"];

            if (refreshToken == null) return Unauthorized();

            var result = await _authService.RefreshAsync(refreshToken);
            CookieInit(result);

            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            await _authService.LogoutAsync(refreshToken!);

            Response.Cookies.Delete("access_token", new CookieOptions
            {
                Secure = false,
                Path = "/",
                SameSite = SameSiteMode.Lax
            });
            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                Secure = false,
                Path = "/",
                SameSite = SameSiteMode.Lax
            });

            return Ok();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            await _authService.ResetPasswordAsync(dto);
            return Ok();
        }

        private void CookieInit(AuthResultDto result)
        {
            Response.Cookies.Append("access_token", result.accessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Path = "/",
                SameSite = SameSiteMode.Lax
            });

            Response.Cookies.Append("refresh_token", result.refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Path = "/",
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddHours(7)
            });
        }
    }
}
