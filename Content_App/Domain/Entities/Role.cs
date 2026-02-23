namespace Content_App.Domain.Entities;

public partial class Role
{
    public Guid Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Descreption { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
