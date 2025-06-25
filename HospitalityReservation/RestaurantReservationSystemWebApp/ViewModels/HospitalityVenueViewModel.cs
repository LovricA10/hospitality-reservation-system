using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class HospitalityVenueViewModel
    {
        public int Idvenue { get; set; }

        [Required(ErrorMessage = "Venue name is required.")]
        [Display(Name = "Venue Name")]
        public string? VenueName { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Type is required.")] 
        [Display(Name = "Type")]
        public int TypeId { get; set; }

        [Display(Name = "Type Name")]
        public string? TypeName { get; set; }
    }
}
