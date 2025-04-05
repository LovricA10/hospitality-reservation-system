using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

[Table("Reservation")]
public partial class Reservation
{
    [Key]
    [Column("IDReservation")]
    public int Idreservation { get; set; }

    public int NumberOfGuests { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateOnly ReservationDate { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("VenueID")]
    public int? VenueId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Reservations")]
    public virtual User? User { get; set; }

    [InverseProperty("Reservation")]
    public virtual ICollection<UserReservation> UserReservations { get; set; } = new List<UserReservation>();

    [ForeignKey("VenueId")]
    [InverseProperty("Reservations")]
    public virtual HospitalityVenue? Venue { get; set; }
}
