using System;
using System.Collections.Generic;

namespace Content_App.Domain.Entities;

public partial class Employee
{
    public Guid Id { get; set; }

    public string EmpCode { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string? Email { get; set; }

    public Guid DeptId { get; set; }

    public Guid AreaId { get; set; }

    public string Grade { get; set; } = null!;

    public virtual ICollection<Action> Actions { get; set; } = new List<Action>();

    public virtual Area Area { get; set; } = null!;

    public virtual Department Dept { get; set; } = null!;
}
