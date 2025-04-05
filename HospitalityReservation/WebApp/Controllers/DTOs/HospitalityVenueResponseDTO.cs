namespace WebApp.Controllers.DTOs
{
    public class HospitalityVenueResponseDTO
    {
        public int Idvenue { get; set; }
        public string? VenueName { get; set; }
        public string? Address { get; set; }
        public int? TypeId { get; set; }
        public string? TypeName { get; set; }
    }
}
