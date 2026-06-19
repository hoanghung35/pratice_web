namespace Content_App.App.DTOs.Order
{
    public class RequestDto
    {
        public Guid id { get; set; }
        public string itemName { get; set; } = null!;
        public string itemName { get; set; } = null!;
        public int qty { get; set; }
        public string kind { get; set; } = null!;
        public string requestorName { get; set; } = null!;
        public string requestorCode { get; set; } = null!;
        public string? reason { get; set; }
        public string dateRequest { get; set; } = null!;
        public string planDate { get; set; } = null!;
        public string status { get; set; } = null!;
    }
}
