using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

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

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserReservation> UserReservations { get; set; }

    public virtual DbSet<VenueMenuItem> VenueMenuItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;User Id=sa;Password=SQL;Database=HospitalityReservationDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HospitalityType>(entity =>
        {
            entity.HasKey(e => e.Idtype).HasName("PK__Hospital__B273339965784E4C");
        });

        modelBuilder.Entity<HospitalityVenue>(entity =>
        {
            entity.HasKey(e => e.Idvenue).HasName("PK__Hospital__271333A79C8E3E84");

            entity.HasOne(d => d.Type).WithMany(p => p.HospitalityVenues)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Hospitali__TypeI__5165187F");
        });

        modelBuilder.Entity<MenuItem>(entity =>
        {
            entity.HasKey(e => e.IdmenuItem).HasName("PK__MenuItem__D5A7F4C4E13C8FF6");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Idreservation).HasName("PK__Reservat__53DF2D8D29A202B2");

            entity.Property(e => e.Status).HasDefaultValue("Pending");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__UserI__571DF1D5");

            entity.HasOne(d => d.Venue).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__Venue__5812160E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PK__User__EAE6D9DFC5E8A055");

            entity.Property(e => e.Role).HasDefaultValue("User");
        });

        modelBuilder.Entity<UserReservation>(entity =>
        {
            entity.HasKey(e => e.IduserReservation).HasName("PK__UserRese__5113F0DF4A4B30C0");

            entity.HasOne(d => d.Reservation).WithMany(p => p.UserReservations).HasConstraintName("FK__UserReser__Reser__71D1E811");

            entity.HasOne(d => d.User).WithMany(p => p.UserReservations).HasConstraintName("FK__UserReser__UserI__70DDC3D8");
        });

        modelBuilder.Entity<VenueMenuItem>(entity =>
        {
            entity.HasKey(e => e.IdvenueMenuItem).HasName("PK__VenueMen__595E1AEF5502B9F9");

            entity.HasOne(d => d.MenuItem).WithMany(p => p.VenueMenuItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__VenueMenu__MenuI__5EBF139D");

            entity.HasOne(d => d.Venue).WithMany(p => p.VenueMenuItems)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__VenueMenu__Venue__5FB337D6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
