using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Models;

public partial class LogEntry
{
    [Key]
    public int Id { get; set; }

    public string Message { get; set; } = null!;

    public int Level { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Timestamp { get; set; }
}
