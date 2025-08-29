using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Models;

public partial class ProjectManagementContext : DbContext
{
    public ProjectManagementContext()
    {
    }

    public ProjectManagementContext(DbContextOptions<ProjectManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectCost> ProjectCosts { get; set; }

    public virtual DbSet<ProjectHistory> ProjectHistories { get; set; }

    public virtual DbSet<ProjectMember> ProjectMembers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DucAnh;Database= ProjectManagement; uid=sa;pwd=sa;Trusted_Connection=True;INtegrated security=true; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8A1F2A9AC");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.UserId, "UQ__Customer__1788CC4DB7311C66").IsUnique();

            entity.Property(e => e.CustomerName).HasMaxLength(200);
            entity.Property(e => e.CustomerType).HasMaxLength(20);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Customer__UserId__36B12243");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Project__761ABEF0740372BD");

            entity.ToTable("Project");

            entity.HasIndex(e => e.ProjectCode, "UQ__Project__2F3A4948ED147908").IsUnique();

            entity.Property(e => e.ActualProgress)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Budget).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Objective).HasMaxLength(500);
            entity.Property(e => e.ProjectCode).HasMaxLength(50);
            entity.Property(e => e.ProjectName).HasMaxLength(200);
            entity.Property(e => e.ProjectType).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.Customer).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Project__Custome__37A5467C");
        });

        modelBuilder.Entity<ProjectCost>(entity =>
        {
            entity.HasKey(e => e.CostId).HasName("PK__ProjectC__8285233E19B8180C");

            entity.ToTable("ProjectCost");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Category).HasMaxLength(30);
            entity.Property(e => e.CostType)
                .HasMaxLength(10)
                .HasDefaultValue("Actual");
            entity.Property(e => e.Note).HasMaxLength(300);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectCosts)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectCo__Proje__38996AB5");
        });

        modelBuilder.Entity<ProjectHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId).HasName("PK__ProjectH__4D7B4ABD4D5E8A03");

            entity.ToTable("ProjectHistory");

            entity.Property(e => e.Action).HasMaxLength(10);
            entity.Property(e => e.ActionDate).HasColumnType("datetime");
            entity.Property(e => e.RoleInProject).HasMaxLength(20);

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectHistories)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectHi__Proje__398D8EEE");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectHi__UserI__3A81B327");
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.HasKey(e => new { e.ProjectId, e.UserId, e.RoleInProject }).HasName("PK__ProjectM__5596DE1C3F15D787");

            entity.ToTable("ProjectMember");

            entity.Property(e => e.RoleInProject).HasMaxLength(20);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");

            entity.HasOne(d => d.Project).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectMe__Proje__3B75D760");

            entity.HasOne(d => d.User).WithMany(p => p.ProjectMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectMe__UserI__3C69FB99");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C297A71EE");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E42EB8659B").IsUnique();

            entity.Property(e => e.Department).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.RoleSystem).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
