using System;
using System.Collections.Generic;

namespace Content_App.Domain.Entities;

public partial class Area
{
    public Guid Id { get; set; }

    public string AreaCode { get; set; } = null!;

    public string AreaName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
