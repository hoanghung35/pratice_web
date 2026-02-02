using System;
using System.Collections.Generic;

namespace Content_App.Domain.Entities;

public partial class OrderItem
{
    public Guid Id { get; set; }

    public Guid ItemId { get; set; }

    public Guid PicId { get; set; }

    public int Qty { get; set; }

    public DateOnly PlanOrder { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime? DateModify { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Account Pic { get; set; } = null!;
}
