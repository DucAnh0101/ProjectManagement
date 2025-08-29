using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int UserId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerType { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual User User { get; set; } = null!;
}
