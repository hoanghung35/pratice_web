using Content_App.Domain.Entities;

namespace Content_App.App.Interfaces
{
    public interface IAuthService
    {
        string GenerateAccessToken(User user);
        RefreshToken GenerateRefreshToken(User user);
    }
}
