namespace Content_App.App.DTOs.Order
{
    public class CreateOrderDto
    {
        public Guid ItemId { get; set; }
        public int Qty { get; set; }
        public DateOnly PlanOrder { get; set; }
        public string? Reason { get; set; }
    }
}
