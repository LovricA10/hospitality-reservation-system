using System.ComponentModel.DataAnnotations;

namespace RestaurantReservationSystemWebApp.ViewModels
{
    public class UserViewModel
    {
        public int Iduser { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string? Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string? Phone { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string? Role { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm password")]
        public string? ConfirmPassword { get; set; }
    }
}
