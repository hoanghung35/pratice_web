using Content_App.App.Interfaces.Authentication;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Content_App.Infrastructure.Security
{
    public class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly AppDbContext _context;

        public RefreshTokenStore(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> FindAsync(string tokenHash)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);
        }

        public async Task RevokeAsync(RefreshToken token)
        {
            token.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
