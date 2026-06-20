using Content_App.App.DTOs.Auth;
using Content_App.App.Interfaces;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Infrastructure.Security;
using Content_App.Shared.Constants;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class AuthService
    {
        private readonly IAuthService _authService;
        private readonly LogDbContext _context;
        private readonly PasswordHasher _hash;
        private readonly RefreshTokenStore _refeshToken;

        

        public AuthService(LogDbContext context, IAuthService authService, PasswordHasher psh, RefreshTokenStore refrestToken)
        {
            this._context = context;
            this._authService = authService;
            this._hash = psh;
            this._refeshToken = refrestToken;
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var account = await _context.Accounts.Include(a => a.Role).FirstOrDefaultAsync(a => a.UserCode == dto.userCode);
            if(account == null || !_hash.Verify(dto.passWord, account.Password))
            {
                throw new UnauthorizedAccessException();
            }
            return GenerateTokens(account);
        }

        //refresh token: day alive - 7
        public async Task<AuthResultDto> RefreshAsync(string refreshToken)
        {
            var userId = _refeshToken.Validate(refreshToken);
            if(userId == null)
            {
                throw new UnauthorizedAccessException();
            }
            _refeshToken.Revoke(refreshToken);
            var account = await _context.Accounts.Include(a => a.Role).FirstOrDefaultAsync(a => a.Id == userId);

            return GenerateTokens(account!);
        }

        public async Task<string> ForgetPasswordAsync(ForgetPasswordDto dto)
        {
            var user = await _context.Employees.FirstOrDefaultAsync(u => u.EmpCode == dto.UserCode && u.Email == dto.Email);
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserCode == dto.UserCode);

            if(user == null || account == null)
            {
                throw new NullReferenceException();
            }

            var newPw = RandomPassword();
            account.Password = _hash.SHA_256Hasher(newPw);
            _context.Accounts.Update(account);

            await _context.SaveChangesAsync();

            return $"{newPw}|{user.Email}|{user.FullName}";
        }

        //logout
        public async Task LogoutAsync(string token)
        {
            if(!string.IsNullOrEmpty(token))
            {
                _refeshToken.Revoke(token);
            }
            await Task.CompletedTask;
        }

        private string RandomPassword()
        {
            return new string(Enumerable.Repeat(VariableConstant.AllowedChars, 5)
                .Select(s => s[Random.Shared.Next(s.Length)])
                .ToArray());
        }
        private AuthResultDto GenerateTokens(Account account)
        {
            var accessToken = _authService.GenerateAccessToken(account);
            var refreshToken = Guid.NewGuid().ToString("N");

            _refeshToken.Store(account.Id, refreshToken, TimeSpan.FromDays(7));

            return new AuthResultDto(accessToken, refreshToken);
        }
    }
}
