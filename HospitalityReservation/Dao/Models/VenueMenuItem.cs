using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Models;

[Table("VenueMenuItem")]
public partial class VenueMenuItem
{
    [Key]
    [Column("IDVenueMenuItem")]
    public int IdvenueMenuItem { get; set; }

    [Column("MenuItemID")]
    public int? MenuItemId { get; set; }

    [Column("VenueID")]
    public int? VenueId { get; set; }

    [ForeignKey("MenuItemId")]
    [InverseProperty("VenueMenuItems")]
    public virtual MenuItem? MenuItem { get; set; }

    [ForeignKey("VenueId")]
    [InverseProperty("VenueMenuItems")]
    public virtual HospitalityVenue? Venue { get; set; }
}
