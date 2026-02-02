using System;
using System.Collections.Generic;
namespace Content_App.Domain.Entities;

public partial class Action
{
    public Guid Id { get; set; }

    public Guid EmpId { get; set; }

    public Guid PicId { get; set; }

    public Guid ItemId { get; set; }

    public int Qty { get; set; }

    public string Status { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateTime? DateAction { get; set; }

    public virtual Employee Emp { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual Account Pic { get; set; } = null!;
}
