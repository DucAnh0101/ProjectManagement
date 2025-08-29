using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ProjectCost
{
    public int CostId { get; set; }

    public int ProjectId { get; set; }

    public string Category { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly CostDate { get; set; }

    public string? Note { get; set; }

    public string? CostType { get; set; }

    public virtual Project Project { get; set; } = null!;
}
