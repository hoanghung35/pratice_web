namespace Content_App.App.DTOs.Item
{
    public class ItemDto
    {
        public Guid? Id { get; set; }
        public Guid DeptId { get; set; }
        public string ItemCode { get; set; ] = null!;
        public string EnName { get; set; } = null!;
    }
}
