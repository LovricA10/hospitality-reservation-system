namespace WebApp.Controllers.DTOs
{
    public class ReservationResponseDTO
    {
        public int Idreservation { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = null!;
        public DateOnly ReservationDate { get; set; }
    }
}
