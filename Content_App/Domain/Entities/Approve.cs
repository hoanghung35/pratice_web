namespace Content_App.Domain.Entities;
public partial class Approve
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid ItemId { get; set; }

    public Guid RequestorId { get; set; }

    public int Qty { get; set; }

    public string Kind { get; set; } = null!;

    public DateTime? DateRequest { get; set; }

    public DateTime? DateApprove { get; set; }

    public string Status { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual OrderItem Order { get; set; } = null!;

    public virtual Account Requestor { get; set; } = null!;
}
