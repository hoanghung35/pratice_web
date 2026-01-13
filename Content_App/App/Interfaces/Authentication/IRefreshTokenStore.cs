using Content_App.Domain.Entities;

namespace Content_App.App.Interfaces.Authentication
{
    public interface IRefreshTokenStore
    {
        Task SaveAsync(RefreshToken token);
        Task<RefreshToken?> FindAsync(string tokenHash);
        Task RevokeAsync(RefreshToken token);
    }
}
