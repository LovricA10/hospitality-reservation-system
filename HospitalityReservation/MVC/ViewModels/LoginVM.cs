using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
