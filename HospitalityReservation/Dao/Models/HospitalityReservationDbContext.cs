using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dao.Models;

public partial class HospitalityReservationDbContext : DbContext
{
    public HospitalityReservationDbContext(DbContextOptions<HospitalityReservationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HospitalityType> HospitalityTypes { get; set; }
    public virtual DbSet<HospitalityVenue> HospitalityVenues { get; set; }
    public virtual DbSet<LogEntry> Logs { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserReservation> UserReservations { get; set; }
    public virtual DbSet<VenueMenuItem> VenueMenuItems { get; set; }
    public virtual DbSet<MenuItem> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.IdmenuItem).HasName("PK__MenuItem__D5A7F4C4E13C8FF6");

            entity.ToTable("MenuItem");

            entity.Property(e => e.IdmenuItem).HasColumnName("IDMenuItem");
            entity.Property(e => e.ItemName).HasMaxLength(100);
            entity.Property(e => e.ItemType).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("money");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
