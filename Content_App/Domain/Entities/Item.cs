namespace Content_App.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public string? ItemCode { get; set; }
        public string? NameVi { get; set; }
        public string? NameEn { get; set; }
        public Guid Area_Id { get; set; }
        public Guid Dept_Id { get; set; }
        public int Quantity { get; set; }
    }
}
