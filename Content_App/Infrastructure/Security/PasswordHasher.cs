using System.Security.Cryptography;
using System.Text;

namespace Content_App.Infrastructure.Security
{
    public class PasswordHasher
    {
        public string SHA_256Hasher(string password)
        {
            using (var schema = SHA256.Create())
            {
                var bytes = schema.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public bool Verify(string login_pw, string password)
        {
            return SHA_256Hasher(login_pw) == password;
        }
    }
}
