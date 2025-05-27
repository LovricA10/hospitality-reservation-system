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

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserReservation> UserReservations { get; set; }

    public virtual DbSet<VenueMenuItem> VenueMenuItems { get; set; }

    public virtual DbSet<LogEntry> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC0774163CD6");

            entity.Property(e => e.Level).HasDefaultValue(1);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getutcdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
