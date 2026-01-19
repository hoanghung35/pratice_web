using Content_App.Domain.Enums;

namespace Content_App.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string UserCode { get; set; } = null!;
        public string Password { get; set; } = null!;
        public RoleCode Role { get; set; }
    }
}
