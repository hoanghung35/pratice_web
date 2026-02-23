using System.Security.Cryptography;
using System.Text;

namespace Content_App.Infrastructure.Security
{
    public class TokenHelper
    {
        public static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public static string HashToken(string token)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(token)));
        }
    }
}
