using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Models;

[Table("HospitalityType")]
[Index("TypeName", Name = "UQ__Hospital__D4E7DFA84CEEB012", IsUnique = true)]
public partial class HospitalityType
{
    [Key]
    [Column("IDType")]
    public int Idtype { get; set; }

    [StringLength(50)]
    public string TypeName { get; set; } = null!;

    [InverseProperty("Type")]
    public virtual ICollection<HospitalityVenue> HospitalityVenues { get; set; } = new List<HospitalityVenue>();
}
