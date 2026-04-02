namespace Content_App.App.DTOs.Order
{
    public class OrderDto
    {
        public Guid id { get; set; }
        public string enName { get; set; } = null!;
        public string vnName { get; set; } = null!;
        public string maker { get; set; } = null!;
        public int quantity { get; set; }
        public string position { get; set; } = null!;
        public string dateOrder { get; set; } = null!;
    }
}
