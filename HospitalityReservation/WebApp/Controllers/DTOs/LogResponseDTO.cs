namespace WebApp.Controllers.DTOs
{
    public class LogResponseDTO
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Level { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
