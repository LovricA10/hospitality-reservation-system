namespace RestaurantReservationSystemWebApi.Controllers.DTOs
{
    public class HospitalityVenueUpdateDTO
    {
        public int Idvenue { get; set; }
        public string? VenueName { get; set; }
        public string? Address { get; set; }
        public int? TypeId { get; set; }
    }
}
