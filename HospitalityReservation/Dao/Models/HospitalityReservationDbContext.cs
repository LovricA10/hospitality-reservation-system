using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserReservation> UserReservations { get; set; }

    public virtual DbSet<VenueMenuItem> VenueMenuItems { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=.;User Id=sa;Password=SQL;Database=HospitalityReservationDB;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HospitalityType>().HasKey(e => e.Idtype);

        modelBuilder.Entity<HospitalityVenue>()
            .HasKey(e => e.Idvenue);
        modelBuilder.Entity<HospitalityVenue>()
            .HasOne(e => e.Type)
            .WithMany(t => t.HospitalityVenues)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuItem>().HasKey(e => e.IdmenuItem);

        modelBuilder.Entity<Reservation>()
            .HasKey(e => e.Idreservation);
        modelBuilder.Entity<Reservation>()
            .Property(e => e.Status)
            .HasMaxLength(20)
            .HasDefaultValue("Pending");
        modelBuilder.Entity<Reservation>()
            .HasOne(e => e.User)
            .WithMany(u => u.Reservations)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Reservation>()
            .HasOne(e => e.Venue)
            .WithMany(v => v.Reservations)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasKey(e => e.Iduser);
        modelBuilder.Entity<User>()
            .Property(e => e.Role)
            .HasDefaultValue("User");

        modelBuilder.Entity<UserReservation>()
            .HasKey(e => e.IduserReservation);
        modelBuilder.Entity<UserReservation>()
            .HasOne(e => e.User)
            .WithMany(u => u.UserReservations);
        modelBuilder.Entity<UserReservation>()
            .HasOne(e => e.Reservation)
            .WithMany(r => r.UserReservations);

        modelBuilder.Entity<VenueMenuItem>()
            .HasKey(e => e.IdvenueMenuItem);
        modelBuilder.Entity<VenueMenuItem>()
            .HasOne(e => e.Venue)
            .WithMany(v => v.VenueMenuItems)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<VenueMenuItem>()
            .HasOne(e => e.MenuItem)
            .WithMany(m => m.VenueMenuItems)
            .OnDelete(DeleteBehavior.Cascade);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
