namespace Content_App.Domain.Entities;
public partial class Currency
{
    public Guid Id { get; set; }

    public string? CurrenName { get; set; }

    public decimal? ExchangeRate { get; set; }
}
