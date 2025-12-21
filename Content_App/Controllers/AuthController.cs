using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Content_App.App.DTOs.Auth;
using Content_App.App.Interfaces;
using Content_App.Infrastructure.Data;
using Org.BouncyCastle.Crypto.Generators;

namespace Content_App.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _auth;

        public AuthController(AppDbContext db, IAuthService auth)
        {
            _db = db;
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
        {
            var user = await _db.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                var accessToken = _auth.GenerateAccessToken(user);
                var refreshToken = _auth.GenerateRefreshToken(user);

                _db.RefreshTokens.Add(refreshToken);
                await _db.SaveChangesAsync();

                Response.Cookies.Append("refresh_token", refreshToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = refreshToken.ExpiresAt
                });

                return Ok(new AuthResponse(accessToken));
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh()
        {
            if (!Request.Cookies.TryGetValue("refresh_token", out var token))
                return Unauthorized();

            var refresh = await _db.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r =>
                    r.Token == token &&
                    !r.Revoked &&
                    r.ExpiresAt > DateTime.UtcNow);

            if (refresh == null)
                return Unauthorized();

            refresh.Revoked = true;

            var newRefresh = _auth.GenerateRefreshToken(refresh.User);
            var newAccess = _auth.GenerateAccessToken(refresh.User);

            _db.RefreshTokens.Add(newRefresh);
            await _db.SaveChangesAsync();

            Response.Cookies.Append("refresh_token", newRefresh.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefresh.ExpiresAt
            });

            return Ok(new AuthResponse(newAccess));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("refresh_token", out var token))
            {
                var rt = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
                if (rt != null)
                {
                    rt.Revoked = true;
                    await _db.SaveChangesAsync();
                }
            }

            Response.Cookies.Delete("refresh_token");
            return Ok();
        }
    }
}
