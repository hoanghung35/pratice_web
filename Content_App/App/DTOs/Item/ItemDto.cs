namespace Content_App.App.DTOs.Item
{
    public class ItemDto
    {
        public Guid? Id { get; set; }
        public Guid DeptId { get; set; }
        public string ItemCode { get; set; ] = null!;
        public string EnName { get; set; } = null!;
        public string? DeptName { get; set; }
        public string VnName { get; set; } = null!;
        public string Maker { get; set; } = null!;
        public string Supplier { get; set; } = null!;
        public string PositionIn { get; set; } = null!;
        public int Quantiry { get; set; }
        public string Unit { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public decimal? Cost { get; set; }
        public string? Image { get; set; }
    }
}
