using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TrafficViolationApp.Models;

public partial class TrafficViolationAppContext : DbContext
{
    public TrafficViolationAppContext()
    {
    }

    public TrafficViolationAppContext(DbContextOptions<TrafficViolationAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Violation> Violations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ARISHEMM;uid=sa;password=123456;database=TrafficViolationApp;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48056E0D1E25");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.PlateNumber).HasMaxLength(50);
            entity.Property(e => e.ReportDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.VideoUrl).HasMaxLength(255);
            entity.Property(e => e.ViolationType).HasMaxLength(100);

            entity.HasOne(d => d.ProcessedByNavigation).WithMany(p => p.ReportProcessedByNavigations)
                .HasForeignKey(d => d.ProcessedBy)
                .HasConstraintName("FK__Reports__Process__4316F928");

            entity.HasOne(d => d.Reporter).WithMany(p => p.ReportReporters)
                .HasForeignKey(d => d.ReporterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__Reporte__4222D4EF");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C25A20383");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053420265AA6").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<Violation>(entity =>
        {
            entity.HasKey(e => e.ViolationId).HasName("PK__Violatio__18B6DC0808EE959B");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValue("Mô t? chua có");
            entity.Property(e => e.FineAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FineDate).HasColumnType("datetime");
            entity.Property(e => e.PlateNumber).HasMaxLength(50);
            entity.Property(e => e.ViolationType)
                .HasMaxLength(100)
                .HasDefaultValue("Chua xác d?nh");

            entity.HasOne(d => d.Report).WithMany(p => p.Violations)
                .HasForeignKey(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Violation__Repor__45F365D3");

            entity.HasOne(d => d.Violator).WithMany(p => p.Violations)
                .HasForeignKey(d => d.ViolatorId)
                .HasConstraintName("FK__Violation__Viola__46E78A0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
