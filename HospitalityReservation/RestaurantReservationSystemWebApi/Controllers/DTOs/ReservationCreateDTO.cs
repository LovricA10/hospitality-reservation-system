namespace RestaurantReservationSystemWebApi.Controllers.DTOs
{
    public class ReservationCreateDTO
    {
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = null!;
        public DateOnly ReservationDate { get; set; }
        public int? UserId { get; set; }
        public int? VenueId { get; set; }
    }
}
