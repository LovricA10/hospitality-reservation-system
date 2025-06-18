using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class HospitalityTypeViewModel
    {
        public int Idtype { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeName { get; set; } = null!;
    }
}
