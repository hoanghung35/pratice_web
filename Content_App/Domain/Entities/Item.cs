using System;
using System.Collections.Generic;

namespace Content_App.Domain.Entities;

public partial class Item
{
    public Guid Id { get; set; }

    public string ItemCode { get; set; } = null!;

    public string? EnName { get; set; }

    public string? VnName { get; set; }

    public int Quantity { get; set; }

    public Guid DeptId { get; set; }

    public Guid AreaId { get; set; }

    public string? Unit { get; set; }

    public decimal? Cost { get; set; }

    public string? Currency { get; set; }

    public string? Maker { get; set; }

    public string? Supplier { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual ICollection<Approve> Approves { get; set; } = new List<Approve>();

    public virtual Area Area { get; set; } = null!;

    public virtual Department Dept { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
