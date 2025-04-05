using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

[Table("HospitalityVenue")]
public partial class HospitalityVenue
{
    [Key]
    [Column("IDVenue")]
    public int Idvenue { get; set; }

    [StringLength(100)]
    public string VenueName { get; set; } = null!;

    [StringLength(255)]
    public string Address { get; set; } = null!;

    [Column("TypeID")]
    public int? TypeId { get; set; }

    [InverseProperty("Venue")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [ForeignKey("TypeId")]
    [InverseProperty("HospitalityVenues")]
    public virtual HospitalityType? Type { get; set; }

    [InverseProperty("Venue")]
    public virtual ICollection<VenueMenuItem> VenueMenuItems { get; set; } = new List<VenueMenuItem>();
}
