using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class LogEntryViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Log Message")]
        public string? Message { get; set; }

        [Display(Name = "Log Level")]
        public int Level { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
