using System;
using System.Collections.Generic;

namespace Content_App.Domain.Entities;

public partial class Account
{
    public Guid Id { get; set; }

    public string UserCode { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid RoleId { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<Approve> Approves { get; set; } = new List<Approve>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual Role Role { get; set; } = null!;
}
