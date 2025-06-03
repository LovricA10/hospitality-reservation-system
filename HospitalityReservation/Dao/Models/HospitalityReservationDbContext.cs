using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Dao.Models;

public partial class HospitalityReservationDbContext : DbContext
{
    public HospitalityReservationDbContext()
    {
    }

    public HospitalityReservationDbContext(DbContextOptions<HospitalityReservationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HospitalityType> HospitalityTypes { get; set; }
    public virtual DbSet<HospitalityVenue> HospitalityVenues { get; set; }
    public virtual DbSet<MenuItem> MenuItems { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<UserReservation> UserReservations { get; set; }
    public virtual DbSet<VenueMenuItem> VenueMenuItems { get; set; }
    public virtual DbSet<LogEntry> Logs { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;User Id=sa;Password=SQL;Database=HospitalityReservationDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Idreservation).HasName("PK__Reservat__53DF2D8D29A202B2");

            entity.Property(e => e.Status).HasDefaultValue("Pending");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
