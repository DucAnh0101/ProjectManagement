using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ProjectHistory
{
    public int HistoryId { get; set; }

    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime ActionDate { get; set; }

    public string? RoleInProject { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
