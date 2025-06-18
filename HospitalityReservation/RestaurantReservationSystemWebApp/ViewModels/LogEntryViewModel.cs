namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class LogEntryViewModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        public int Level { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
