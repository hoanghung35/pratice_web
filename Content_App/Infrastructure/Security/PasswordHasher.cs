namespace Content_App.Infrastructure.Security
{
    public class PasswordHasher
    {
        public bool Verify(string pw1, string pw2)
        {
            return pw1 == pw2;
        }
    }
}
