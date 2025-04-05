using System.ComponentModel.DataAnnotations;

namespace WebApp.Controllers.DTOs
{
    public class UserDTO
    {
    
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name should be between 2 and 50 characters long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name should be between 2 and 50 characters long")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Provide a correct e-mail address")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Provide a correct phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("User|Admin", ErrorMessage = "Role must be 'User' or 'Admin'")]
        public string Role { get; set; } = "User";
    }
}
