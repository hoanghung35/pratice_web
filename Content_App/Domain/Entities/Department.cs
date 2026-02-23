namespace Content_App.Domain.Entities;

public partial class Department
{
    public Guid Id { get; set; }

    public string DeptCode { get; set; } = null!;

    public string DeptName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
