using Microsoft.Extensions.Caching.Memory;

namespace Content_App.Infrastructure.Security
{
    public class RefreshTokenStore
    {
        private readonly IMemoryCache _cache;

        public RefreshTokenStore(IMemoryCache cache)
        {
            this._cache = cache;
        }

        public void Store(Guid userId, string refreshToken, TimeSpan t)
        {
            _cache.Set(GetKey(refreshToken), userId, t);
        }

        public Guid? Validate(string refreshToken)
        {
            var key = GetKey(refreshToken);
            return _cache.TryGetValue(GetKey(refreshToken), out Guid userId) ? userId : null;
        }

        public void Revoke(string refreshToken)
        {
            _cache.Remove(GetKey(refreshToken));
        }

        private static string GetKey(string refreshToken) => $"refresh_token: {refreshToken}";
    }
}
