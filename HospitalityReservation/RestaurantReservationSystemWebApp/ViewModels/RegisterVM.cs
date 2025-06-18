using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class RegisterVM
    {


        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
