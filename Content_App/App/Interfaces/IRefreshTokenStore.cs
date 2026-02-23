using Content_App.Domain.Entities;

namespace Content_App.App.Interfaces
{
    public interface IRefreshTokenStore
    {
        Task<RefreshToken?> FindAsync(string tokenHash);
        Task RevokeAsync(RefreshToken token);
        Task SaveAsync(RefreshToken token);
    }
}
