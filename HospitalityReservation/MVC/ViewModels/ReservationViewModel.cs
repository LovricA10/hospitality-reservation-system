using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.ViewModels
{
    public class ReservationViewModel
    {
        public int Idreservation { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int VenueId { get; set; }
        public string? VenueName { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime ReservationDate { get; set; }
        public string? Status { get; set; }

    }
}
