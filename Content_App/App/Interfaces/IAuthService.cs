using Content_App.Domain.Entities;

namespace Content_App.App.Interfaces
{
    public interface IAuthService
    {
        string GenerateAccessToken(Account account);
    }
}
