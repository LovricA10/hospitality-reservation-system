using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApi.Controllers.DTOs
{
    public class ReservationUpdateDTO
    {
        public int NumberOfGuests { get; set; }

        public string Status { get; set; } = null!;

        public DateOnly ReservationDate { get; set; }
    }
}
