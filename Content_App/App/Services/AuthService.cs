using Content_App.App.DTOs.Auth;
using Content_App.App.Interfaces;
using Content_App.App.Interfaces.Authentication;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Content_App.App.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly PasswordHasher _hasher;
        private readonly RefreshTokenStore _refreshTokenStore;

        public AuthService(AppDbContext context, IJwtService jwtService, PasswordHasher hasher, RefreshTokenStore refreshTokenStore)
        {
            _context = context;
            _jwtService = jwtService;
            _hasher = hasher;
            _refreshTokenStore = refreshTokenStore;
        }

        // LOGIN
        public async Task<AuthResultDto> LoginAsync(LoginRequest dto)
        {
            var account = await _context.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.UserCode == dto.Username);

            if (account == null || !_hasher.Verify(dto.Password, account.Password))
                throw new UnauthorizedAccessException("Invalid credentials");

            return GenerateTokens(account);
        }

        // REFRESH + ROTATION
        public async Task<AuthResultDto> RefreshAsync(string refreshToken)
        {
            var userId = _refreshTokenStore.Validate(refreshToken);
            if (userId == null)
                throw new UnauthorizedAccessException("Invalid refresh token");

            // rotate
            _refreshTokenStore.Revoke(refreshToken);

            var account = await _context.Accounts
                .Include(x => x.Role)
                .FirstAsync(x => x.Id == userId);

            return GenerateTokens(account);
        }

        //logout
        public async Task LogoutAsync(string refreshToken)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                _refreshTokenStore.Revoke(refreshToken);
            }

            await Task.CompletedTask;
        }


        private AuthResultDto GenerateTokens(Account account)
        {
            var accessToken = _jwtService.GenerateToken(account);
            var refreshToken = Guid.NewGuid().ToString("N");

            _refreshTokenStore.Store(
                account.Id,
                refreshToken,
                TimeSpan.FromDays(7)
            );

            return new AuthResultDto(accessToken, refreshToken);
        }
    }
}
