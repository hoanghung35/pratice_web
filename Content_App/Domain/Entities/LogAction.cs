namespace Content_App.Domain.Entities;
public partial class LogAction
{
    public Guid Id { get; set; }

    public string EmpCode { get; set; } = null!;

    public Guid PicId { get; set; }

    public Guid ItemId { get; set; }

    public int Qty { get; set; }

    public string Kind { get; set; } = null!;

    public string Reason { get; set; } = null!;

    public DateTime? DateAction { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Account Pic { get; set; } = null!;
}
