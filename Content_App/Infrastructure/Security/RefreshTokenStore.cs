using Content_App.App.Interfaces.Authentication;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Content_App.Infrastructure.Security
{
    public class RefreshTokenStore
    {
        private readonly IMemoryCache _cache;

        public RefreshTokenStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Store(Guid userId, string refreshToken, TimeSpan ttl)
        {
            _cache.Set(GetKey(refreshToken), userId, ttl);
        }

        public Guid? Validate(string refreshToken)
        {
            return _cache.TryGetValue(GetKey(refreshToken), out Guid userId)
                ? userId
                : null;
        }

        public void Revoke(string refreshToken)
        {
            _cache.Remove(GetKey(refreshToken));
        }

        private static string GetKey(string token) => $"refresh_token:{token}";
    }
}
