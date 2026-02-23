namespace Content_App.App.DTOs.Item
{
    public class ItemActivityDto
    {
        public Guid ItemId { get; set; }
        public Guid PicId { get; set; }
        public string? EmpCode { get; set; }
        public int Qty { get; set; }
    }
}
