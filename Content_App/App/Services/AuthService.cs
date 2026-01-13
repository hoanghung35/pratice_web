using Content_App.App.Interfaces;
using Content_App.App.Interfaces.Authentication;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Security;


namespace Content_App.App.Services
{
    public class AuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenStore _refreshStore;

        public AuthService(
            IJwtService jwtService,
            IRefreshTokenStore refreshStore)
        {
            _jwtService = jwtService;
            _refreshStore = refreshStore;
        }

        public async Task<(string accessToken, string refreshToken)>
            GenerateTokenPairAsync(Account account)
        {
            var accessToken = _jwtService.GenerateAccessToken(account);

            var refreshToken = TokenHelper.GenerateRefreshToken();
            var hashed = TokenHelper.Hash(refreshToken);

            await _refreshStore.SaveAsync(new RefreshToken
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                TokenHash = hashed,
                ExpiresAt = DateTime.UtcNow.AddDays(14)
            });

            return (accessToken, refreshToken);
        }
    }
}
