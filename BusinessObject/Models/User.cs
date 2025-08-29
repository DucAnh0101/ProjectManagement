using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string RoleSystem { get; set; } = null!;

    public string? Department { get; set; }

    public bool IsActive { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<ProjectHistory> ProjectHistories { get; set; } = new List<ProjectHistory>();

    public virtual ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
}
