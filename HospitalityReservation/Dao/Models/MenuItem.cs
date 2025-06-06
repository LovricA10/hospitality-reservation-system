using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Models;

public partial class MenuItem
{
    [Key]
    [Column("IDMenuItem")]
    public int IdmenuItem { get; set; }

    [StringLength(100)]
    public string ItemName { get; set; } = null!;

    [StringLength(50)]
    public string ItemType { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    public string? ImageBase64 { get; set; }

    [InverseProperty("MenuItem")]
    public virtual ICollection<VenueMenuItem> VenueMenuItems { get; set; } = new List<VenueMenuItem>();
}
