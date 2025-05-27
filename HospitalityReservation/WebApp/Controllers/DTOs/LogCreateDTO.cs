namespace WebApp.Controllers.DTOs
{
    public class LogCreateDTO
    {
        public string Message { get; set; } = string.Empty;
        public int Level { get; set; } = 1;
    }
}
