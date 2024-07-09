using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClaimsPortalAPI.Models;

public partial class ClaimsPortalDbContext : DbContext
{
    public ClaimsPortalDbContext()
    {
    }

    public ClaimsPortalDbContext(DbContextOptions<ClaimsPortalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<PolicyHolder> PolicyHolders { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=ClaimsPortalDB;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasIndex(e => e.PolicyHolderId, "IX_Policies_PolicyHolderId");

            entity.HasIndex(e => e.VehicleId, "IX_Policies_VehicleId");

            entity.Property(e => e.CoverageAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PremiumAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.PolicyHolder).WithMany(p => p.Policies).HasForeignKey(d => d.PolicyHolderId);

            //entity.HasOne(d => d.Vehicle).WithMany(p => p.Policies)
            //    .HasForeignKey(d => d.VehicleId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PolicyHolder>(entity =>
        {
            entity.Property(e => e.Zip).HasColumnName("ZIP");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasIndex(e => e.PolicyHolderId, "IX_Vehicles_PolicyHolderId");

            entity.Property(e => e.Vin).HasColumnName("VIN");

            entity.HasOne(d => d.PolicyHolder).WithMany(p => p.Vehicles).HasForeignKey(d => d.PolicyHolderId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
