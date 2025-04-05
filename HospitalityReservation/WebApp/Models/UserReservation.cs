using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

[Table("UserReservation")]
public partial class UserReservation
{
    [Key]
    [Column("IDUserReservation")]
    public int IduserReservation { get; set; }

    [Column("UserID")]
    public int? UserId { get; set; }

    [Column("ReservationID")]
    public int? ReservationId { get; set; }

    [ForeignKey("ReservationId")]
    [InverseProperty("UserReservations")]
    public virtual Reservation? Reservation { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserReservations")]
    public virtual User? User { get; set; }
}
