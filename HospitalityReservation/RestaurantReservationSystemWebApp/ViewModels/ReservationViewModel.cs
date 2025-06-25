using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class ReservationViewModel
    {
        public int Idreservation { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Display(Name = "User name")]
        public string? UserName { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [Display(Name = "Venue name")]
        public string? VenueName { get; set; }

        [Required(ErrorMessage = "Number of guests is required.")]
        [Range(1, 100, ErrorMessage = "Number of guests must be between 1 and 100.")]
        [Display(Name = "Number of guests")]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Reservation date is required.")] 
        [Display(Name = "Reservation date")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Status")]
        public string? Status { get; set; }

    }
}
