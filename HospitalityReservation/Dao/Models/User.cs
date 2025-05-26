using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Dao.Models;

[Table("User")]
[Index("Email", Name = "UQ__User__A9D10534D1A0F565", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("IDUser")]
    public int Iduser { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? Phone { get; set; }

    [StringLength(20)]
    public string Role { get; set; } = null!;

    [StringLength(256)]
    public string PwdHash { get; set; } = null!;

    [StringLength(256)]
    public string PwdSalt { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    [InverseProperty("User")]
    public virtual ICollection<UserReservation> UserReservations { get; set; } = new List<UserReservation>();
}
