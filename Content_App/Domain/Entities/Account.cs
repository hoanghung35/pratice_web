using Content_App.Domain.Enums;

namespace Content_App.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public RoleCode Role { get; set; }
    }
}
