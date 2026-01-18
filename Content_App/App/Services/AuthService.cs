using Content_App.App.DTOs.Auth;
using Content_App.App.Interfaces;
using Content_App.App.Interfaces.Authentication;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;


namespace Content_App.App.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwt;
        private readonly PasswordHasher _hasher;

        public AuthService(AppDbContext context, IJwtService jwt, PasswordHasher hasher)
        {
            _context = context;
            _jwt = jwt;
            _hasher = hasher;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest dto)
        {
            var account = await _context.Accounts
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.UserCode == dto.UserCode);

            if (account == null || !_hasher.Verify(dto.Password, account.Password))
                throw new UnauthorizedAccessException();

            var token = _jwt.GenerateToken(account);
            return new AuthResponse(token);
        }
    }
}
