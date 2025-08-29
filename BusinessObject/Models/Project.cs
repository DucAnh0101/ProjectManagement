using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Project
{
    public int ProjectId { get; set; }

    public string ProjectCode { get; set; } = null!;

    public string ProjectName { get; set; } = null!;

    public int CustomerId { get; set; }

    public string ProjectType { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Objective { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = null!;

    public bool IsActive { get; set; }

    public decimal? Budget { get; set; }

    public decimal? ActualProgress { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<ProjectCost> ProjectCosts { get; set; } = new List<ProjectCost>();

    public virtual ICollection<ProjectHistory> ProjectHistories { get; set; } = new List<ProjectHistory>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
}
