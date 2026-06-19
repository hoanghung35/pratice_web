namespace Content_App.App.DTOs.Order
{
    public class CreateRequestDto
    {
        public Guid itemId { get; set; }
        public int qty { get; set; }
        public DateTime? planRequest { get; set; }
        public string? Reason { get; set; }
        public string? Ucode { get; set; }
        public string? Uname { get; set; }
    }
}
