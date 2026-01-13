using Content_App.Domain.Entities;

namespace Content_App.App.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(Account account);
    }

}
